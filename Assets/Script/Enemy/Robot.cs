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

    /// <summary></summary>
    private const float ROTATION_DURATION = 1.5f;

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
            if (IsArrivedAtPatrolDestination())
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

    /// <summary>巡回する目標地点に到達しているかどうか</summary>
    private bool IsArrivedAtPatrolDestination()
    {
        return Vector3.Distance(transform.position, _patrolDestination.Value) 
            < ARRIVAL_THRESHOLD ? true : false;
    }

    /// <summary>巡回する目標地点をランダムに設定する</summary>
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

    /// <summary>目標地点の方向へ回転させるコルーチン</summary>
    IEnumerator RotateTowardsDestination()
    {
        // 目標地点が存在する場合
        if (_patrolDestination.HasValue)
        {
            // 目標地点へのベクトルを求める
            var dir = (_patrolDestination.Value - transform.position).normalized;

            // ベクトルを基に、目標地点への回転方向を求める
            Quaternion lookRotation = Quaternion.LookRotation(dir);

            // 目標地点へ回転中であることを示すフラグをオンにする
            _isRotating = true;

            // 目標地点の方向へ回転させる
            Tween rotationTween = transform.DORotate(lookRotation.eulerAngles, 1.5f);

            // 回転の完了を待機する
            yield return rotationTween.WaitForCompletion();

            // 目標地点へ回転中であることを示すフラグをオフにする
            _isRotating = false;
        }
        // 目標地点が存在しない場合
        else
        {
            // エラーログを出力して処理を抜ける
            Debug.LogError("Destination not Set.");
            yield break;
        }
    }
}
