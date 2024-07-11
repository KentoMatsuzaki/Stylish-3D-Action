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
        // 入力値が閾値（Press）以上になった場合
        if (context.performed)
        {
            // ジャンプさせる
            _jumpControl.Jump(true);
        }
    }
}
