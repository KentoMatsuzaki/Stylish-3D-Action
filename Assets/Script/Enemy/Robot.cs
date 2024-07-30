using DG.Tweening;
using System.Collections;
using UnityEngine;

/// <summary>空陸両用ロボット</summary>
public class Robot : MonoBehaviour
{
    /// <summary>アニメーター</summary>
    Animator _animator;

    CharacterController _controller;

    Vector3? _destination;
    float _patrolRange = 5f;
    float _moveSpeed = 1f;
    float _arrivalThreshold = 0.5f;

    float timer = 0f;
    bool _isRotating = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Patrol();
        timer += Time.deltaTime;

        if (timer > 1f && _destination.HasValue)
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
                if (!_isRotating)
                {
                    MoveForward();
                }   
            }
        }
        else
        {
            SetRandomDestination();
            StartCoroutine(RotateTowardsDestination());
        }
        return NodeStatus.Running;
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

    /// <summary>前方に移動させる</summary>
    private void MoveForward()
    {
        _controller.Move(transform.forward * _moveSpeed * Time.deltaTime);
    }

    IEnumerator RotateTowardsDestination()
    {
        if (_destination.HasValue)
        {
            var dir = (_destination.Value - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            _isRotating = true;

            Tween rotationTween = transform.DORotate(lookRotation.eulerAngles, 1.5f);
            yield return rotationTween.WaitForCompletion();

            _isRotating = false;
        }
        else
        {
            Debug.LogError("Destination not Set.");
            yield break;
        }
    }
}
