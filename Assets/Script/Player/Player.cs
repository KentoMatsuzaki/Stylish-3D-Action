using UnityEngine;
using UnityEngine.InputSystem;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using UnityEngine.UI;


/// <summary>プレイヤーの状態管理</summary>
public class Player : MonoBehaviour
{
    /// <summary>移動制御</summary>
    private MoveControl _moveControl;

    /// <summary>ジャンプ制御</summary>
    private JumpControl _jumpControl;

    /// <summary>接地判定</summary>
    private GroundCheck _groundCheck;

    /// <summary>アニメーター</summary>
    private Animator _animator;

    /// <summary>現在のプレイヤーの状態</summary>
    private PlayerState _currentState = PlayerState.Idle;

    /// <summary>歩行時の移動速度</summary>
    [SerializeField, Header("歩行時の移動速度")] private float _walkSpeed = 4.0f;

    /// <summary>走行時の移動速度</summary>
    [SerializeField, Header("走行時の移動速度")] private float _sprintSpeed = 7.5f;

    /// <summary>ルートモーションを適用するか</summary>
    private bool _applyRootMotion = false;

    /// <summary>右手の攻撃クラス</summary>
    [SerializeField, Header("右手の攻撃クラス")] private PlayerAttacker _rightSwordAttacker;

    /// <summary>左手の攻撃クラス</summary>
    [SerializeField, Header("左手の攻撃クラス")] private PlayerAttacker _leftSwordAttacker;

    /// <summary>攻撃判定の持続時間</summary>
    [SerializeField, Header("攻撃判定の持続時間")] private float _attackDuration;



    [SerializeField] Text _text;

    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
        _groundCheck = GetComponent<GroundCheck>();
        _jumpControl = GetComponent<JumpControl>();
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

        // 入力値が閾値（Press）以上になった場合
        if (context.performed)
        {
            // 入力値を元に計算した移動方向へと移動させる
            _moveControl.Move(context.ReadValue<Vector2>());

            // 移動状態に遷移できる場合
            if (CanTransitionToMoveState())
            {
                // アニメーションを再生して、プレイヤーの状態を更新する
                _animator.Play("Move");
                _currentState = PlayerState.Move;
            }
        }

        // 入力値が閾値（Release）以下になった場合
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
        if (_currentState == PlayerState.Attack || _currentState == PlayerState.Damage ||
            _currentState == PlayerState.Die) return false;
        else return true;
    }

    //-------------------------------------------------------------------------------
    // スプリントのコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>スプリントの制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnSprint(InputAction.CallbackContext context)
    {
        // 入力値が閾値（Press）以上になった場合
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

        // 入力値が閾値（Release）以下になった場合
        else if (context.canceled)
        {
            // 歩行時の移動速度に変更する
            _moveControl.MoveSpeed = _walkSpeed;

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

        // 入力値が閾値（Press）以上になった場合
        if (context.performed)
        {
            // ジャンプさせる
            _jumpControl.Jump(true);

            // アニメーションを再生して、プレイヤーの状態を更新する
            _animator.Play("Jump Start");
            _currentState = PlayerState.Jump;
        }
    }

    /// <summary>着地時の処理をするコールバックイベント</summary>
    /// <summary>Gravityコンポーネントから呼ばれる</summary>
    public void OnLand()
    {
        // アニメーションを再生して、プレイヤーの状態を更新する
        _animator.Play("Jump End");

        if (_animator.GetBool("IsSprint")) _currentState = PlayerState.Sprint;
        else _currentState = _moveControl.IsMove ? PlayerState.Move : PlayerState.Idle;
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
        // 攻撃アクションが長押しされた場合
        if (context.performed)
        {
            // 攻撃状態に遷移できる場合
            if (CanTransitionToAttackState())
            {
                // トリガーを有効化して、プレイヤーの状態を更新する
                _animator.SetTrigger("Attack");
                _currentState = PlayerState.Attack;
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
            case 0:
                TriggerRightSwordCollider();
                break;

            case 1:
                TriggerLeftSwordCollider();
                break;

            case 2:
                TriggerRightSwordCollider();
                TriggerLeftSwordCollider();
                break;

            default:
                Debug.LogError($"Unexpected handIndex value : {handIndex}");
                break;
        }
    }

    /// <summary>右手で持っている武器のコライダーを一時的に有効化する</summary>
    private void TriggerRightSwordCollider()
    {
        EnableRightSwordCollider();
        Invoke(nameof(DisableRightSwordCollider), _attackDuration);
    }

    /// <summary>左手で持っている武器のコライダーを一時的に有効化する</summary>
    private void TriggerLeftSwordCollider()
    {
        EnableLeftSwordCollider();
        Invoke(nameof(DisableLeftSwordCollider), _attackDuration);
    }

    public void EnableRightSwordCollider() => _rightSwordAttacker.EnableCollider();

    public void DisableRightSwordCollider() => _rightSwordAttacker.DisableCollider();

    public void EnableLeftSwordCollider() => _leftSwordAttacker.EnableCollider();

    public void DisableLeftSwordCollider() => _leftSwordAttacker.DisableCollider();
}
