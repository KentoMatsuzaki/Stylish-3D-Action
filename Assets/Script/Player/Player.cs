using System;
using System.Collections.Generic;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using UnityEngine;
using UnityEngine.InputSystem;

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

    /// <summary>プレイヤーの状態</summary>
    private State _currentState;

    /// <summary>状態のコレクション</summary>
    private Dictionary<Type, State> _stateDic;

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

        _stateDic = new Dictionary<Type, State>
        {
            { typeof(IdleState), new IdleState(this) },
            { typeof(TrotState), new TrotState(this) },
            { typeof(SprintState), new SprintState(this) },
            { typeof(JumpState), new JumpState(this) },
            { typeof(AttackState), new AttackState(this) },
            { typeof(DamageState), new DamageState(this) },
            { typeof(DeadState), new DeadState(this) },
        };

        ChangeState<IdleState>();
    }

    /// <summary>プレイヤーの状態を遷移させる</summary>
    private void ChangeState<T>() where T : State
    {
        // 現在の状態と遷移先の状態が同じ場合は早期リターン
        if (_currentState is T) return;

        // 状態を遷移させる
        _currentState?.Exit();
        _currentState = _stateDic[typeof(T)];
        _currentState.Enter();
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

            // 状態を遷移
            if(!(_currentState is SprintState)) ChangeState<TrotState>();
        }

        // 入力値が閾値（Release）以下になった場合
        else if (context.canceled)
        { 
            // 移動しないようにする
            _moveControl.Move(Vector2.zero);

            // 状態を遷移
            ChangeState<IdleState>();
        }
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

            // 状態を遷移
            ChangeState<SprintState>();
        }

        // 入力値が閾値（Release）以下になった場合
        else if (context.canceled)
        {
            // 歩行時の移動速度に変更する
            _moveControl.MoveSpeed = _walkSpeed;

            // 状態を遷移
            if (_moveControl.IsMove) ChangeState<TrotState>();
            else ChangeState<IdleState>();
        }
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
        }
    }
}
