using UnityEngine;
using DG.Tweening;
using System.Collections;

/// <summary>空陸両用ロボット</summary>
public class Robot : MonoBehaviour
{
    /// <summary>アニメーター</summary>
    Animator _animator;

    /// <summary>キャラクターコントローラー</summary>
    CharacterController _controller;

    /// <summary>巡回する目標地点</summary>
    Vector3? _patrolDestination;

    /// <summary>巡回する目標地点を求める際の球の半径</summary>
    [SerializeField, Header("巡回する範囲の半径")]  private float _patrolRadius = 5f;

    /// <summary>巡回する際の移動速度</summary>
    [SerializeField, Header("巡回時の移動速度")] private float _patrolSpeed = 1f;

    /// <summary>巡回する目標地点に到達したかどうかを判定する閾値</summary>
    private const float ARRIVAL_THRESHOLD = 0.5f;

    /// <summary>巡回する目標地点の方向へ回転中であることを示すフラグ</summary>
    private bool _isRotating = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Patrol();
    }

    //-------------------------------------------------------------------------------
    // 巡回
    //-------------------------------------------------------------------------------

    /// <summary>目標地点を巡回させる</summary>
    /// <returns>巡回アクションノードの評価結果</returns>
    public NodeStatus Patrol()
    {
        // 目標地点が存在する場合
        if (_patrolDestination.HasValue)
        {
            // 目標地点に到達した場合
            if (IsArrivedAtDestination())
            {
                // 目標地点をクリアして、成功の評価結果を返す
                _patrolDestination = null;
                return NodeStatus.Success;
            }
            // 目標地点に到達していない場合
            else
            {
                // 目標地点の方向へ回転中である場合
                if (!_isRotating)
                {
                    // 前方へ移動させる
                    MoveForward();
                }   
            }
        }
        // 目標地点が存在しない場合
        else
        {
            // ランダムな目標地点を設定する
            SetRandomDestination();

            // 目標地点の方向へ回転させる
            StartCoroutine(RotateTowardsDestination());
        }

        // 実行中の評価結果を返す
        return NodeStatus.Running;
    }

    //-------------------------------------------------------------------------------
    // 巡回に関する処理
    //-------------------------------------------------------------------------------

    /// <summary>巡回の目標地点に到達しているかどうか</summary>
    private bool IsArrivedAtDestination()
    {
        return Vector3.Distance(transform.position, _patrolDestination.Value) 
            < ARRIVAL_THRESHOLD ? true : false;
    }

    /// <summary>ランダムな巡回の目標地点を設定する</summary>
    private void SetRandomDestination()
    {
        // 巡回範囲を半径とする球内の、ランダムな地点を取得する
        Vector3 randomPos = Random.insideUnitSphere * _patrolRadius;

        // ランダムな地点のY座標を0に設定する
        randomPos.y = 0;

        // ランダムな地点は原点を中心としているため、キャラクターの位置を加算する
        randomPos += transform.position;

        // 目的地をランダムな地点に設定する
        _patrolDestination = randomPos;
    }

    /// <summary>前方に移動させる</summary>
    private void MoveForward()
    {
        _controller.Move(transform.forward * _patrolSpeed * Time.deltaTime);
    }

    IEnumerator RotateTowardsDestination()
    {
        if (_patrolDestination.HasValue)
        {
            var dir = (_patrolDestination.Value - transform.position).normalized;
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
