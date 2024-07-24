using UnityEngine;
using UnityEngine.InputSystem;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using UnityEngine.UI;
using Unity.TinyCharacterController.Brain;
using Unity.TinyCharacterController.Effect;

/// <summary>プレイヤーの状態管理</summary>
public class Player : MonoBehaviour
{
    /// <summary>移動制御</summary>
    private MoveControl _moveControl;

    /// <summary>ジャンプ制御</summary>
    private JumpControl _jumpControl;

    /// <summary>接地判定</summary>
    private GroundCheck _groundCheck;

    /// <summary>重力制御</summary>
    private Gravity _gravity;

    /// <summary>ロジック制御</summary>
    private CharacterBrain _brain;

    /// <summary>アニメーター</summary>
    private Animator _animator;

    /// <summary>現在のプレイヤーの状態</summary>
    private PlayerState _currentState = PlayerState.Idle;

    /// <summary>歩行時の移動速度</summary>
    [SerializeField, Header("歩行時の移動速度")] private float _defaultSpeed = 4.0f;

    /// <summary>走行時の移動速度</summary>
    [SerializeField, Header("走行時の移動速度")] private float _sprintSpeed = 7.5f;

    /// <summary>右手の攻撃クラス</summary>
    [SerializeField, Header("右手の攻撃クラス")] private PlayerAttacker _rightSwordAttacker;

    /// <summary>左手の攻撃クラス</summary>
    [SerializeField, Header("左手の攻撃クラス")] private PlayerAttacker _leftSwordAttacker;

    /// <summary>攻撃判定の持続時間</summary>
    [SerializeField, Header("攻撃判定の持続時間")] private float _attackDuration;

    /// <summary>攻撃の属性</summary>
    [SerializeField, Header("攻撃の属性")] public AttackEffectType _attackEffectType;

    /// <summary>浮遊に消費されるエネルギー</summary>
    [SerializeField, Header("浮遊エネルギー")] private float _floatEnergy = 1f;

    /// <summary>敵を探索する範囲の半径</summary>
    [SerializeField, Header("敵を探索する範囲の半径")] private float _detectionRadius = 5f;

    /// <summary>敵のレイヤーマスク</summary>
    [SerializeField, Header("敵のレイヤーマスク")] private LayerMask _enemyLayer;

    public float ALT_ATTACK_Y_POS = 3f;

    [SerializeField] Text _text;

    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
        _groundCheck = GetComponent<GroundCheck>();
        _jumpControl = GetComponent<JumpControl>();
        _gravity = GetComponent<Gravity>();
        _brain = GetComponent<CharacterBrain>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // 移動フラグを設定
        _animator.SetBool("IsMove", _moveControl.IsMove);

        // ルートモーションの適用フラグを設定
        _animator.applyRootMotion = _currentState == PlayerState.Attack ? true : false;

        // テスト用
        _text.text = _currentState.ToString();
    }

    //-------------------------------------------------------------------------------
    // プレイヤーの状態
    //-------------------------------------------------------------------------------

    private enum PlayerState
    {
        Idle, 　// 無操作
        Move, 　// 移動
        Sprint, // スプリント
        Jump, 　// ジャンプ
        Attack, // 攻撃
        Float, // 浮遊
        Damage, // ダメージ
        Die 　　// 死亡
    }

    //-------------------------------------------------------------------------------
    // プレイヤーの状態に関する処理
    //-------------------------------------------------------------------------------

    /// <summary>プレイヤーの状態を無操作状態に設定する</summary>
    /// <summary>アニメーションイベントから呼ばれる</summary>
    public void SetPlayerStateIdle ()
    {
        _currentState = PlayerState.Idle;
    }

    /// <summary>プレイヤーの状態を移動状態に設定する</summary>
    /// <summary>アニメーションイベントから呼ばれる</summary>
    public void SetPlayerStateMove()
    {
        _currentState = PlayerState.Move;
    }

    //-------------------------------------------------------------------------------
    // 移動のコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>移動の制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 移動を受け付けない場合は処理を抜ける
        if(!CanMove()) return;

        // 移動アクションに割り当てられたキーバインドが入力された場合
        if (context.performed)
        {
            // 入力値の移動方向へ移動させる
            _moveControl.Move(context.ReadValue<Vector2>());

            // 移動状態に遷移できる場合
            if (CanTransitionToMoveState())
            {
                // アニメーションを再生して、プレイヤーの状態を更新する
                _animator.Play("Move");
                _currentState = PlayerState.Move;
            }
        }

        // 移動アクションに割り当てられたキーバインドが離された場合
        else if (context.canceled)
        {
            // 移動方向を0にする（移動させないようにする）
            _moveControl.Move(Vector2.zero);

            // 移動状態の場合
            if (_currentState == PlayerState.Move)
            {
                // アニメーションを再生して、プレイヤーの状態を更新する
                _animator.Play("Move End");
                _currentState = PlayerState.Idle;
            }

            // スプリント状態の場合
            else if(_currentState == PlayerState.Sprint)
            {
                // アニメーションを再生して、プレイヤーの状態を更新する
                _animator.Play("Sprint End");
                _currentState = _moveControl.IsMove ? PlayerState.Move : PlayerState.Idle;
            }
        }
    }

    //-------------------------------------------------------------------------------
    // 移動に関する処理
    //-------------------------------------------------------------------------------

    /// <summary>移動状態に遷移できるかどうか</summary>
    private bool CanTransitionToMoveState()
    {
        if (_currentState == PlayerState.Idle) return true;
        return false;
    }

    /// <summary>移動を受け付けるかどうか</summary>
    private bool CanMove()
    {
        if (_currentState == PlayerState.Attack || _currentState == PlayerState.Float || 
            _currentState == PlayerState.Damage || _currentState == PlayerState.Die) return false;
        else return true;
    }

    //-------------------------------------------------------------------------------
    // スプリントのコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>スプリントの制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnSprint(InputAction.CallbackContext context)
    {
        // スプリントアクションに割り当てられたキーバインドが入力された場合
        if (context.performed)
        {
            // 走行時の移動速度に変更する
            _moveControl.MoveSpeed = _sprintSpeed;

            // フラグを有効化する
            _animator.SetBool("IsSprint", true);

            // スプリント状態に遷移できる場合
            if(CanTransitionToSprintState())
            {
                // アニメーションを再生して、プレイヤーの状態を更新する
                _animator.Play("Sprint");
                _currentState = PlayerState.Sprint;
            }
        }

        // スプリントアクションに割り当てられたキーバインドが離された場合
        else if (context.canceled)
        {
            // 歩行時の移動速度に変更する
            _moveControl.MoveSpeed = _defaultSpeed;

            // フラグを無効化する
            _animator.SetBool("IsSprint", false);

            // スプリント状態の場合
            if(_currentState == PlayerState.Sprint)
            {
                // アニメーションを再生して、プレイヤーの状態を更新する
                _animator.Play("Sprint End");
                _currentState = _moveControl.IsMove ? PlayerState.Move : PlayerState.Idle;
            } 
        }
    }

    //-------------------------------------------------------------------------------
    // スプリントに関する処理
    //-------------------------------------------------------------------------------

    /// <summary>スプリント状態に遷移できるかどうか</summary>
    private bool CanTransitionToSprintState()
    {
        if (_currentState == PlayerState.Move) return true;
        return false;
    }

    //-------------------------------------------------------------------------------
    // ジャンプのコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>ジャンプの制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // ジャンプ状態に遷移できる場合
        if (!CanTransitionToJumpState()) return;

        // ジャンプアクションに割り当てられたキーバインドが入力された場合
        if (context.performed)
        {
            // ジャンプさせる
            _jumpControl.Jump(true);

            // アニメーションを再生して、プレイヤーの状態を更新する
            _animator.Play("Jump");
            _currentState = PlayerState.Jump;
        }
    }

    /// <summary>着地時の処理をするコールバックイベント</summary>
    /// <summary>Gravityコンポーネントから呼ばれる</summary>
    public void OnLand()
    {
        // アニメーションを再生して、プレイヤーの状態を更新する
        _animator.Play("Land");
        _currentState = !_moveControl.IsMove ? PlayerState.Idle :
            _moveControl.MoveSpeed == _defaultSpeed ? PlayerState.Move : PlayerState.Sprint;

        _jumpControl.JumpHeight = 2f;
        _gravity.GravityScale = 1.5f;
    }

    //-------------------------------------------------------------------------------
    // ジャンプに関する処理
    //-------------------------------------------------------------------------------

    /// <summary>ジャンプ状態に遷移できるかどうか</summary>
    private bool CanTransitionToJumpState()
    {
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Move ||
            _currentState == PlayerState.Sprint) return true;
        return false;
    }

    //-------------------------------------------------------------------------------
    // 攻撃のコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>攻撃の制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnAttack(InputAction.CallbackContext context)
    {
        // 攻撃アクションに割り当てられたキーバインドが入力された場合
        if (context.performed)
        {
            // 攻撃状態に遷移できる場合
            if (CanTransitionToAttackState())
            {
                // 攻撃トリガーを有効化して、プレイヤーの状態を更新する
                _animator.SetTrigger("Attack");
                _currentState = PlayerState.Attack;

                LookAtClosestEnemy();
            }  
        }
    }

    //-------------------------------------------------------------------------------
    // 攻撃に関する処理
    //-------------------------------------------------------------------------------

    /// <summary>攻撃状態に遷移できるかどうか</summary>
    private bool CanTransitionToAttackState()
    {
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Attack) return true;
        return false;
    }

    /// <summary>攻撃がヒットする瞬間の処理をするコールバックイベント</summary>
    /// <summary>アニメーションイベントから呼ばれる</summary>
    /// <param name="handIndex">攻撃に用いる手を示すインデックス（0が右手、1が左手、2が両手）</param>
    public void AttackImpactEvent(int handIndex)
    {
        switch(handIndex)
        {
            // 右手
            case 0:
                TriggerRightSwordCollider();
                break;

            // 左手
            case 1:
                TriggerLeftSwordCollider();
                break;

            // 両手
            case 2:
                TriggerRightSwordCollider();
                TriggerLeftSwordCollider();
                break;

            default:
                Debug.LogError($"Unexpected handIndex value : {handIndex}");
                break;
        }
    }

    /// <summary>右手で持っている武器のコライダーを持続時間だけ有効化する</summary>
    private void TriggerRightSwordCollider()
    {
        EnableRightSwordCollider();
        Invoke(nameof(DisableRightSwordCollider), _attackDuration);
    }

    /// <summary>左手で持っている武器のコライダーを持続時間だけ有効化する</summary>
    private void TriggerLeftSwordCollider()
    {
        EnableLeftSwordCollider();
        Invoke(nameof(DisableLeftSwordCollider), _attackDuration);
    }

    public void EnableRightSwordCollider() => _rightSwordAttacker.EnableCollider();

    public void DisableRightSwordCollider() => _rightSwordAttacker.DisableCollider();

    public void EnableLeftSwordCollider() => _leftSwordAttacker.EnableCollider();

    public void DisableLeftSwordCollider() => _leftSwordAttacker.DisableCollider();

    /// <summary>斬撃エフェクトを生成・表示する</summary>
    /// <summary>アニメーションイベントから呼ばれる</summary>
    /// <param name="handIndex">攻撃に用いる手を示すインデックス（0が右手、1が左手、2が両手）</param>
    public void PlaySlashEffect(int handIndex)
    {
        // プレイヤーの座標を元に、正しい位置にエフェクトを生成する
        Vector3 playerPos = transform.position;
        var effectPos = new Vector3(playerPos.x, playerPos.y + 1.25f, playerPos.z);
        EffectManager.Instance.PlaySlashEffect(_attackEffectType, effectPos, transform, handIndex);
    }

    /// <summary>探索範囲内でプレイヤーから最も近い位置にいる敵を探索する</summary>
    /// <returns>最も近い敵の位置</returns>
    private Transform FindClosestEnemy()
    {
        // プレイヤーの位置を中心に、指定された半径内にいる全ての敵を取得する
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, _detectionRadius, _enemyLayer);
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (var enemy in enemyColliders)
        {
            // プレイヤーと敵の位置の差を計算し、距離の平方を求める
            var distanceSqr = (enemy.transform.position - transform.position).sqrMagnitude;

            // 現在の最短距離と比較して、より近い敵が見つかった場合、最短距離と最も近い敵を更新する
            if (distanceSqr < closestDistanceSqr)
            {
                closestEnemy = enemy.transform;
                closestDistanceSqr = distanceSqr;
            }
        }

        // 最も近い敵が見つかった場合、その敵のTransformを返す。
        if (closestEnemy != null)
        {
            return closestEnemy;
        }
        // 見つからなかった場合、ログを出力してnullを返す。
        else
        {
            Debug.Log("No enemy found in detection range.");
            return null;
        }
    }

    /// <summary>最も近い敵がいる方向にプレイヤーを回転させる</summary>
    private void LookAtClosestEnemy()
    {
        // 最も近い敵の位置を取得する
        Transform closestEnemyPos = FindClosestEnemy();

        // 最も近い敵が見つからなかった場合、ログを出力して処理を抜ける
        if (closestEnemyPos == null)
        {
            Debug.Log("No enemy found to look at.");
            return;
        }

        // プレイヤーと敵の位置の差を計算し、目標となる座標を求める
        Vector3 targetPosition = FindClosestEnemy().position - transform.position;

        // 目標となる座標までの回転量を求める
        Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

        // プレイヤーを回転させる
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 100f);
    }

    //-------------------------------------------------------------------------------
    // 浮遊のコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>浮遊の制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnFloat(InputAction.CallbackContext context)
    {
        // 浮遊アクションに割り当てられたキーバインドが入力された場合
        if (context.performed)
        {
            // 浮遊状態に遷移できる場合
            if (CanTransitionToFloatState())
            {
                // 浮遊させる
                _jumpControl.JumpHeight = 3f;
                _jumpControl.Jump();

                // アニメーションを再生して、プレイヤーの状態を更新する
                _animator.Play("Float Jump");
                _currentState = PlayerState.Float;
            }

            // 浮遊状態である場合
            else if(_currentState == PlayerState.Float)
            {
                _brain.SetFreezeAxis(false, false, false);
            }
        }
    }

    //-------------------------------------------------------------------------------
    // 浮遊に関する処理
    //-------------------------------------------------------------------------------

    /// <summary>浮遊状態に遷移できるかどうか</summary>
    private bool CanTransitionToFloatState()
    {
        if (_currentState == PlayerState.Idle && _floatEnergy > 0) return true;
        return false;
    }

    /// <summary>浮遊イベント</summary>
    /// <summary>アニメーションイベントから呼ばれる</summary>
    public void Float()
    {
        FreezeYAxis();
        WeakenGravity();
    }

    /// <summary>プレイヤーのY座標を固定する</summary>
    private void FreezeYAxis()
    {
        _brain.SetFreezeAxis(false, true, false);
    }

    /// <summary>重力を弱める</summary>
    private void WeakenGravity()
    {
        _gravity.GravityScale = 0.5f;
    }
}
