using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;

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
    
    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
        _groundCheck = GetComponent<GroundCheck>();
        _jumpControl = GetComponent<JumpControl>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (_currentState)
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
        }
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

    }

    /// <summary>歩行中の処理</summary>
    private void HandleWalkingState()
    {

    }

    /// <summary>走行中の処理</summary>
    private void HandleRunningState()
    {

    }

    /// <summary>ジャンプ中の処理</summary>
    private void HandleJumpingState()
    {

    }

    /// <summary>攻撃状態の処理</summary>
    private void HandleAttackState()
    {

    }

    /// <summary>被ダメージ状態の処理</summary>
    private void HandleDamagedState()
    {

    }

    /// <summary>死亡状態の処理</summary>
    private void HandleDeadState()
    {

    }

    /// <summary>状態の遷移</summary>
    private void TransitionState(PlayerState newState)
    {
        _currentState = newState;
    }

    //-------------------------------------------------------------------------------
    // 移動のコールバックイベント
    //-------------------------------------------------------------------------------

    /// <summary>移動の物理演算を行う</summary>
    /// <summary>PlayerInputから呼ばれる</summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 入力値が閾値（Press）以上になった場合
        if (context.performed)
        {
            // 移動方向を計算
            _moveControl.Move(context.ReadValue<Vector2>());
        }

        // 入力値が閾値（Release）以下になった場合
        else if (context.canceled)
        { 
            // 移動方向を無効化
            _moveControl.Move(Vector2.zero);
        }

        TransitionState(PlayerState.Walking);
    }
}
