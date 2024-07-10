using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>プレイヤーの状態管理</summary>
public class PlayerStateMachine : MonoBehaviour
{
    /// <summary>移動制御</summary>
    private MoveControl _moveControl;

    /// <summary>ジャンプ制御</summary>
    private JumpControl _jumpControl;

    /// <summary>接地判定</summary>
    private GroundCheck _groundCheck;

    /// <summary>アニメーター</summary>
    private Animator _animator;

    /// <summary>プレイヤーの状態</summary>
    private PlayerState _currentState = PlayerState.Idle;

    /// <summary>歩行時の移動速度</summary>
    [SerializeField, Header("歩行時の移動速度")] private float _walkSpeed = 1.2f;

    /// <summary>走行時の移動速度</summary>
    [SerializeField, Header("走行時の移動速度")] private float _sprintSpeed = 4.0f;

    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
        _groundCheck = GetComponent<GroundCheck>();
        _jumpControl = GetComponent<JumpControl>();
        _animator = GetComponent<Animator>();
    }

    //-------------------------------------------------------------------------------
    // ステートマシン
    //-------------------------------------------------------------------------------

    private enum PlayerState
    {
        Idle, // 無操作状態
        Walking, // 歩行中
        Running, // 走行中
        Jumping, // ジャンプ中
        Attack, // 攻撃状態
        Damaged, // 被ダメージ状態
        Dead // 死亡状態
    }

    /// <summary>無操作状態の処理</summary>
    private void HandleIdleState()
    {
        Debug.Log("プレイヤーの状態：Idle");
    }

    /// <summary>歩行中の処理</summary>
    private void HandleWalkingState()
    {
        Debug.Log("プレイヤーの状態：Walking");
    }

    /// <summary>走行中の処理</summary>
    private void HandleRunningState()
    {
        Debug.Log("プレイヤーの状態：Running");
    }

    /// <summary>ジャンプ中の処理</summary>
    private void HandleJumpingState()
    {
        Debug.Log("プレイヤーの状態：Jumping");
    }

    /// <summary>攻撃状態の処理</summary>
    private void HandleAttackState()
    {
        Debug.Log("プレイヤーの状態：Attack");
    }

    /// <summary>被ダメージ状態の処理</summary>
    private void HandleDamagedState()
    {
        Debug.Log("プレイヤーの状態：Damaged");
    }

    /// <summary>死亡状態の処理</summary>
    private void HandleDeadState()
    {
        Debug.Log("プレイヤーの状態：Dead");
    }

    /// <summary>プレイヤーの状態を遷移させる</summary>
    private void TransitionToOtherState(PlayerState otherState)
    {
        // 現在の状態と遷移先の状態が同じ場合、処理を抜ける
        if (_currentState == otherState) return;

        // 現在の状態を遷移させる
        _currentState = otherState;

        // イベントを呼ぶ
        OnStateTransition();
    }

    /// <summary>各状態の固有処理を実行するイベント。状態の遷移時に呼ばれる。</summary>
    private void OnStateTransition()
    {
        switch(_currentState)
        {
            case PlayerState.Idle:
                HandleIdleState(); 
                break;

            case PlayerState.Walking:
                HandleWalkingState();
                break;

            case PlayerState.Running:
                HandleRunningState();
                break;

            case PlayerState.Jumping:
                HandleJumpingState();
                break;

            case PlayerState.Attack:
                HandleAttackState();
                break;

            case PlayerState.Damaged:
                HandleDamagedState();
                break;

            case PlayerState.Dead:
                HandleDeadState();
                break;
        }
    }

    //-------------------------------------------------------------------------------
    // 移動のコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>移動の制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 歩行状態に遷移する
        TransitionToOtherState(PlayerState.Walking);

        // 入力値が閾値（Press）以上になった場合
        if (context.performed)
        {
            // 入力値を元に計算した移動方向へと移動させる
            _moveControl.Move(context.ReadValue<Vector2>());
        }

        // 入力値が閾値（Release）以下になった場合
        else if (context.canceled)
        { 
            // 移動しないようにする
            _moveControl.Move(Vector2.zero);
        }
    }

    //-------------------------------------------------------------------------------
    // スプリントのコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>スプリントの制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnSprint(InputAction.CallbackContext context)
    {
        // 走行状態に遷移する
        TransitionToOtherState(PlayerState.Running);

        // 入力値が閾値（Press）以上になった場合
        if (context.performed)
        {
            // 走行時の移動速度に変更する
            _moveControl.MoveSpeed = _sprintSpeed;
        }

        // 入力値が閾値（Release）以下になった場合
        else if (context.canceled)
        {
            // 歩行時の移動速度に変更する
            _moveControl.MoveSpeed = _walkSpeed;
        }
    }

    //-------------------------------------------------------------------------------
    // ジャンプのコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>ジャンプの制御をするコールバックイベント</summary>
    /// <summary>PlayerInputコンポーネントから呼ばれる</summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // ジャンプ状態に遷移する
        TransitionToOtherState(PlayerState.Jumping);

        // 入力値が閾値（Press）以上になった場合
        if (context.performed)
        {
            // ジャンプさせる
            _jumpControl.Jump(true);
        }
    }
}
