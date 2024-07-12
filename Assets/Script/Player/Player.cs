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

    /// <summary>コントローラー</summary>
    private CharacterController _controller;

    /// <summary>現在のプレイヤーの状態</summary>
    private PlayerState _currentState = PlayerState.Idle;

    /// <summary>歩行時の移動速度</summary>
    [SerializeField, Header("歩行時の移動速度")] private float _walkSpeed = 1.2f;

    /// <summary>走行時の移動速度</summary>
    [SerializeField, Header("走行時の移動速度")] private float _sprintSpeed = 4.0f;

    /// <summary>疑似的にルートモーションを再現するか</summary>
    private bool _applyRootMotion = false;



    [SerializeField] Text _text;

    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
        _groundCheck = GetComponent<GroundCheck>();
        _jumpControl = GetComponent<JumpControl>();
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        _text.text = _currentState.ToString();
    }

    private void LateUpdate()
    {
        MoveWithRootMotion();
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
    // 移動のコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>移動の制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値が閾値（Press）以上になった場合
        if (context.performed)
        {
            // 入力値を元に計算した移動方向へと移動させる
            _moveControl.Move(context.ReadValue<Vector2>());

            // 移動状態に遷移できる場合
            if (CanTransitionToMoveState())
            {
                // アニメーションを再生
                _animator.Play("Move");

                // プレイヤーの状態を更新
                _currentState = PlayerState.Move;
            }
        }

        // 入力値が閾値（Release）以下になった場合
        else if (context.canceled)
        {
            // 移動しないようにする
            _moveControl.Move(Vector2.zero);

            // 移動状態の場合
            if (_currentState == PlayerState.Move)
            {
                // アニメーションを再生
                _animator.Play("Move End");

                // プレイヤーの状態を更新
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
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Move) return true;
        return false;
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

            // スプリント状態に遷移できる場合
            if(CanTransitionToSprintState())
            {
                // アニメーションを再生
                _animator.Play("Sprint");

                // プレイヤーの状態を更新
                _currentState = PlayerState.Sprint;
            }
        }

        // 入力値が閾値（Release）以下になった場合
        else if (context.canceled)
        {
            // 歩行時の移動速度に変更する
            _moveControl.MoveSpeed = _walkSpeed;

            // スプリント状態の場合
            if(_currentState == PlayerState.Sprint)
            {
                // アニメーションを再生
                _animator.Play("Sprint End");

                // プレイヤーの状態を更新
                _currentState = PlayerState.Idle;
            } 
        }
    }

    //-------------------------------------------------------------------------------
    // スプリントに関する処理
    //-------------------------------------------------------------------------------

    /// <summary>スプリント状態に遷移できるかどうか</summary>
    private bool CanTransitionToSprintState()
    {
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Move ||
            _currentState == PlayerState.Sprint) return true;
        return false;
    }

    //-------------------------------------------------------------------------------
    // ジャンプのコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>ジャンプの制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // 入力値が閾値（Press）以上になった場合
        if (context.performed)
        {
            // ジャンプさせる
            _jumpControl.Jump(true);

            // ジャンプ状態ではない場合
            if (_currentState != PlayerState.Jump)
            {
                // アニメーションを再生
                _animator.Play("Jump Start");

                // プレイヤーの状態を更新
                _currentState = PlayerState.Jump;
            }
        }
    }

    /// <summary>着地時の処理をするコールバックイベント</summary>
    /// <summary>Gravityコンポーネントから呼ばれる</summary>
    public void OnLand()
    {
        // アニメーションを再生
        _animator.Play("Jump End");

        // プレイヤーの状態を更新
        _currentState = PlayerState.Idle;
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
            _animator.SetTrigger("Attack");
        }
    }

    //-------------------------------------------------------------------------------
    // 攻撃に関する処理
    //-------------------------------------------------------------------------------

    /// <summary>攻撃のヒット処理をするコールバックイベント</summary>
    /// <summary>アニメーションイベントから呼ばれる</summary>
    public void AttackImpactEvent()
    {
        
    }

    /// <summary>ルートモーションの適用フラグを切り替える</summary>
    /// <summary>アニメーションイベントから呼ばれる</summary>
    public void SwitchRootMotionFlag()
    {
        _applyRootMotion = !_applyRootMotion;
    }

    /// <summary>ルートモーションに合わせてプレイヤーを移動させる</summary>
    public void MoveWithRootMotion()
    {
        if (_applyRootMotion) _controller.Move(transform.forward * Time.deltaTime * 3f);
    }
}
