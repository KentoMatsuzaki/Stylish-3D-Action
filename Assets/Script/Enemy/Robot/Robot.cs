using UnityEngine;
using DG.Tweening;
using System.Collections;

/// <summary>空陸両用ロボット</summary>
public class Robot : MonoBehaviour
{ 
    [SerializeField, Header("巡回アクションの設定項目")] private PatrolSettings _patrolSettings;

    [SerializeField, Header("追跡アクションの設定項目")] private ChaseSettings _chaseSettings;

    /// <summary>アニメーター</summary>
    Animator _animator;

    /// <summary>キャラクターコントローラー</summary>
    CharacterController _controller;

    /// <summary>巡回する目的地</summary>
    Vector3? _patrolDestination;

    /// <summary>レイキャストを飛ばす位置のY座標のオフセット</summary>
    private const float RAYCAST_Y_OFFSET = 0.75f;

    /// <summary>巡回する目的地へ回転中であるかを示すフラグ</summary>
    private bool _isRotatingTowardsPatrolDestination = false;

    /// <summary>初期化アニメーションの完了を示すフラグ</summary>
    private bool _isInitialized = false;

    public float PlayerDetectionRange => _chaseSettings._playerDetectionRange;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 初期化アニメーションの再生が終了していない場合は、処理を抜ける
        if (!_isInitialized) return;

        if (HasPlayerDetected())
        {
            Chase();
        }
        else
        {
            Patrol();
        }
    }

    //-------------------------------------------------------------------------------
    // BehaviorTreeに関連しない処理
    //-------------------------------------------------------------------------------

    /// <summary>初期化アニメーションが完了したことを示すフラグをオンにする</summary>
    /// <summary>アニメーションイベントから呼ばれる</summary>
    public void SetIsInitializedTrue()
    {
        _isInitialized = true;
    }

    //-------------------------------------------------------------------------------
    // 条件ノード
    //-------------------------------------------------------------------------------

    /// <summary>プレイヤーを感知しているかどうかを返す</summary>
    private bool HasPlayerDetected()
    {
        return GetHorizontalDistanceToPlayer() < _chaseSettings._playerDetectionRange;
    }

    //-------------------------------------------------------------------------------
    // 条件ノードに関する処理
    //-------------------------------------------------------------------------------

    /// <summary>Y座標を無視したプレイヤーとの距離を求める</summary>
    private float GetHorizontalDistanceToPlayer()
    {
        // プレイヤーの位置を取得する
        Vector3 playerPos = Player.Instance.transform.position;

        // 自身の位置を取得する
        Vector3 currentPos = transform.position;

        // Y座標を無視した距離を求めて返す
        return Vector3.Distance(new Vector3(playerPos.x, 0, playerPos.z), 
            new Vector3(currentPos.x, 0, currentPos.z));
    }

    //-------------------------------------------------------------------------------
    // 巡回
    //-------------------------------------------------------------------------------

    /// <summary>目的地を巡回する<summary>
    /// <returns>巡回アクションノードの評価結果</returns>
    private NodeStatus Patrol()
    {
        // 目的地が存在する場合
        if (_patrolDestination.HasValue)
        {
            // 目的地に到達した場合
            if (HasReachedPatrolDestination())
            {
                // 目的地をクリアして、成功の評価結果を返す
                ClearPatrolDestination();
                return NodeStatus.Success;
            }

            // 目的地へ回転している途中でない場合
            if (!_isRotatingTowardsPatrolDestination)
            {
                // 前方に障害物が存在する場合、目的地をクリアする
                if (HasObstacleInFront()) ClearPatrolDestination();

                // 前方へ移動させる
                MoveForward();
            }
        }
        // 目的地が存在しない場合
        else
        {
            // ランダムな目的地を割り当てる
            AssignRandomPatrolDestination();

            // 目的地の方向へ回転させる
            StartCoroutine(RotateTowardsPatrolDestination());
        }

        // 実行中の評価結果を返す
        return NodeStatus.Running;
    }

    //-------------------------------------------------------------------------------
    // 巡回に関する処理
    //-------------------------------------------------------------------------------
    
    /// <summary>巡回する目的地との距離を求める</summary>
    private float GetDistanceToPatrolDestination()
    {
        return Vector3.Distance(transform.position, _patrolDestination.Value);
    }

    /// <summary>巡回する目的地へ到達したかどうかを返す</summary>
    private bool HasReachedPatrolDestination()
    {
        return GetDistanceToPatrolDestination() < _patrolSettings._patrolArrivalThreshold;
    }

    /// <summary>巡回する目的地をクリアする</summary>
    private void ClearPatrolDestination()
    {
        _patrolDestination = null;
    }

    /// <summary>前方に障害物が存在するかどうかを返す</summary>
    private bool HasObstacleInFront()
    {
        // レイキャストを飛ばす位置を取得
        Vector3 raycastPos = new Vector3(transform.position.x, RAYCAST_Y_OFFSET, transform.position.z);

        // レイキャストを前方に飛ばし、衝突の有無を返す
        return Physics.Raycast(raycastPos, transform.forward, _patrolSettings._raycastDistance);
    }

    /// <summary>前方に移動させる</summary>
    private void MoveForward()
    {
        _controller.Move(transform.forward * _patrolSettings._patrolSpeed * Time.deltaTime);
    }

    /// <summary>巡回する目的地をランダムに割り当てる</summary>
    private void AssignRandomPatrolDestination()
    {
        Vector3 randomPos;

        // ランダムな地点を取得する
        do randomPos = GetRandomPositionInsidePatrolRange();

        // ランダムな地点への回転角度が最小回転角度を上回るまで繰り返す
        while (!IsValidRotationAngle(randomPos));

        // ランダムな地点を目的地に設定する
        _patrolDestination = randomPos;
    }

    /// <summary>巡回範囲内にあるランダムな地点を取得する</summary>
    private Vector3 GetRandomPositionInsidePatrolRange()
    {
        // 巡回範囲を半径とする円内にあるランダムな地点を取得する
        Vector3 randomPos = Random.insideUnitSphere * _patrolSettings._patrolRange;

        // ランダムな地点のY座標を0にする
        randomPos.y = 0;

        // ランダムな地点は原点を中心としているため、自身の位置を加算する
        return randomPos + transform.position;
    }

    /// <summary>引数で指定した地点への方向を求める</summary>
    private Vector3 GetDirectionToPosition(Vector3 position)
    {
        return (position - transform.position).normalized;
    }

    /// <summary>引数で指定した地点への回転角度を求める</summary>
    private float GetRotationAngleToPosition(Vector3 position)
    {
        return Vector3.Angle(transform.forward, GetDirectionToPosition(position));
    }

    /// <summary>引数で指定した地点への回転角度が最小回転角度を上回るかどうかを返す</summary>
    private bool IsValidRotationAngle(Vector3 position)
    {
        return GetRotationAngleToPosition(position) > _patrolSettings._minRotationAngle;
    }

    /// <summary>目標地点の方向へ回転させるコルーチン</summary>
    IEnumerator RotateTowardsPatrolDestination()
    {
        // 目標地点が存在する場合
        if (_patrolDestination.HasValue)
        {
            // 目的地への回転を行うフラグをオンにする
            _isRotatingTowardsPatrolDestination = true;

            // 目的地への回転を求める
            Quaternion rotation = GetRotationToPosition(_patrolDestination.Value);

            // 目的地の方向へ回転させる
            Tween rotationTween = transform.DORotate(rotation.eulerAngles, _patrolSettings._patrolRotationDuration);

            // 回転の完了を待機する
            yield return rotationTween.WaitForCompletion();

            // 回転が完了した後にフラグをオフにする
            _isRotatingTowardsPatrolDestination = false;
            
        }
        // 目標地点が存在しない場合
        else
        {
            // エラーログを出力する
            Debug.LogError("Destination not Set.");
            yield break;
        }
    }

    /// <summary>引数で指定した地点への回転を求める</summary>
    private Quaternion GetRotationToPosition(Vector3 position)
    {
        return Quaternion.LookRotation(GetDirectionToPosition(position));
    }

    /// <summary>レイキャストを可視化させる</summary>
    private void OnDrawGizmos()
    {
        // ギズモの色を設定する
        Gizmos.color = Color.red;

        // 現在のキャラクターの位置を取得する
        Vector3 currentPos = transform.position;

        // レイキャストを飛ばす位置を設定する
        Vector3 raycastPos = new Vector3(currentPos.x, RAYCAST_Y_OFFSET, currentPos.z);

        // レイキャストを表示する
        Gizmos.DrawRay(raycastPos, transform.forward * _patrolSettings._raycastDistance);
    }

    //-------------------------------------------------------------------------------
    // 追跡
    //-------------------------------------------------------------------------------

    /// <summary>対象（プレイヤー）を追跡する</summary>
    /// <returns>追跡アクションノードの評価結果</returns>
    private NodeStatus Chase()
    {
        // 巡回目標をクリアする
        _patrolDestination = null;

        // 追跡対象の元に到達した場合、成功の評価結果を返す
        if (HasReachedChaseTarget()) return NodeStatus.Success;

        // プレイヤーの方へ回転させる
        RotateTowardsPlayer();

        // プレイヤーの方向へ移動させる
        _controller.Move(transform.forward * _chaseSettings._chaseSpeed * Time.deltaTime);

        // 実行中の評価結果を返す
        return NodeStatus.Running;
    }

    //-------------------------------------------------------------------------------
    // 追跡に関する処理
    //-------------------------------------------------------------------------------

    /// <summary>プレイヤーとの距離を求める</summary>
    private float GetDistanceToPlayer()
    {
        return Vector3.Distance(transform.position, Player.Instance.transform.position);
    }

    /// <summary>追跡対象の元へ到達したかどうかを返す</summary>
    private bool HasReachedChaseTarget()
    {
        return GetDistanceToPlayer() < _chaseSettings._chaseArrivalThreshold;
    }

    /// <summary>Y座標を無視したプレイヤーへの回転を求める</summary>
    private Quaternion GetHorizontalRotationToPlayer()
    {
        // Y座標を無視したプレイヤーの位置を取得する
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 fixedPlayerPos = new Vector3(playerPos.x, 0, playerPos.z);

        return GetRotationToPosition(fixedPlayerPos);
    }

    /// <summary>プレイヤーの方向へ回転させる</summary>
    private void RotateTowardsPlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, GetHorizontalRotationToPlayer(), 
            _chaseSettings._chaseRotationSLerpSpeed * Time.deltaTime);
    }
}