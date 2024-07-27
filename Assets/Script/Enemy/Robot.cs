using UnityEngine;

/// <summary>空陸両用ロボット</summary>
public class Robot : MonoBehaviour
{
    /// <summary>アニメーター</summary>
    Animator _animator;

    /// <summary>コントローラー</summary>
    CharacterController _controller;

    /// <summary>ロボットの現在の状態</summary>
    RobotState _currentState;

    /// <summary>ロボットの状態を表す列挙型</summary>
    private enum RobotState
    {
        /// <summary>巡回状態</summary>
        Patrol,

        /// <summary>追跡状態</summary>
        Chase,

        /// <summary>攻撃状態</summary>
        Attack,

        /// <summary>被ダメージ状態</summary>
        Damage,

        /// <summary>死亡状態</summary>
        Dead
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    //-------------------------------------------------------------------------------
    // 巡回
    //-------------------------------------------------------------------------------

    //public NodeStatus Patrol()
    //{
        
    //}

    //-------------------------------------------------------------------------------
    // 巡回に関する処理
    //-------------------------------------------------------------------------------
}
