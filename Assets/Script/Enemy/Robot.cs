using UnityEngine;

/// <summary>空陸両用ロボット</summary>
public class Robot : MonoBehaviour
{
    /// <summary>アニメーター</summary>
    Animator _animator;

    /// <summary>ロボットの現在の状態</summary>
    RobotState _currentState;

    CharacterController _controller;

    Vector3? _destination;
    float _patrolRange = 5f;
    float _moveSpeed = 1f;
    float _arrivalThreshold = 0.5f;

    float timer = 0f;

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

    private void Update()
    {
        Patrol();
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            Debug.Log(_destination.Value.ToString());
            Debug.Log(Vector3.Distance(transform.position, _destination.Value));
            timer = 0f;
        }
    }

    //-------------------------------------------------------------------------------
    // 巡回
    //-------------------------------------------------------------------------------

    public NodeStatus Patrol()
    {
        if (_destination.HasValue)
        {
            if (IsArrivedAtDestination())
            {
                _destination = null;
                return NodeStatus.Success;
            }
            else
            {
                _controller.Move(transform.forward * _moveSpeed * Time.deltaTime);
                return NodeStatus.Running;
            }
        }
        else
        {
            SetRandomDestination();
            return NodeStatus.Running;
        }
    }

    //-------------------------------------------------------------------------------
    // 巡回に関する処理
    //-------------------------------------------------------------------------------

    /// <summary>巡回の目標地点に到達しているかどうか</summary>
    private bool IsArrivedAtDestination()
    {
        return Vector3.Distance(transform.position, _destination.Value) 
            < _arrivalThreshold ? true : false;
    }

    /// <summary>ランダムな巡回の目標地点を設定する</summary>
    private void SetRandomDestination()
    {
        // 巡回範囲を半径とする球内の、ランダムな地点を取得する
        Vector3 randomPos = Random.insideUnitSphere * _patrolRange;

        // ランダムな地点のY座標を0に設定する
        randomPos.y = 0;

        // ランダムな地点は原点を中心としているため、キャラクターの位置を加算する
        randomPos += transform.position;

        // 目的地をランダムな地点に設定する
        _destination = randomPos;
    }
}
