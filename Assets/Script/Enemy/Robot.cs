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
    [SerializeField, Header("巡回する範囲の半径")] private float _patrolRadius = 5f;

    /// <summary>巡回する際の移動速度</summary>
    [SerializeField, Header("巡回時の移動速度")] private float _patrolSpeed = 1f;

    /// <summary>プレイヤーを感知する距離</summary>
    private const float DETECTION_RANGE = 2.5f;

    /// <summary>巡回する目標地点に到達したかどうかを判定する閾値</summary>
    private const float ARRIVAL_THRESHOLD = 0.5f;

    /// <summary>巡回する目標地点の方向への回転に要する時間</summary>
    private const float ROTATION_DURATION = 1.5f;

    /// <summary>目標地点を新たに設定する際の最小回転角度</summary>
    private const float MIN_ANGLE = 45f;

    /// <summary>レイキャストを飛ばす位置のY座標のオフセット</summary>
    private const float RAYCAST_Y_OFFSET = 0.75f;

    /// <summary>レイキャストを飛ばす距離</summary>
    private const float RAYCAST_DISTANCE = 1.5f;

    /// <summary>巡回する目標地点の方向へ回転中であることを示すフラグ</summary>
    private bool _isRotating = false;

    /// <summary>初期化アニメーションが完了したことを示すフラグ</summary>
    private bool _isInitialized = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // 初期化アニメーションの再生が終了していない場合は、処理を抜ける
        if (!_isInitialized) return;

        Patrol();
    }

    //-------------------------------------------------------------------------------
    // BehaviorTreeに関連しない処理
    //-------------------------------------------------------------------------------

    /// <summary>初期化アニメーションが完了したことを示すフラグをオンにする</summary>
    public void SetIsInitializedTrue()
    {
        _isInitialized = true;
    }

    //-------------------------------------------------------------------------------
    // 条件ノード
    //-------------------------------------------------------------------------------

    /// <summary>プレイヤーを感知しているか</summary>
    /// <returns>プレイヤーが感知範囲内にいる場合はtrue、そうでない場合はfalse</returns>
    private bool IsPlayerDetected()
    {
        // プレイヤーのインスタンスが存在しない場合
        if (Player.Instance == null)
        {
            // エラーログを出力してfalseを返す
            Debug.LogError("Player instance not found.");
            return false;
        }

        // プレイヤーとの距離が感知距離よりも小さい場合はtrueを返す
        float distance = Vector3.Distance(transform.position, Player.Instance.transform.position);
        return distance < DETECTION_RANGE;
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
                    // 衝突判定をチェックし、目標地点をクリアする
                    CheckCollisionThenClearDestination();

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
        // 目標地点への回転角度
        float rotationAngle; 

        // ランダムな地点
        Vector3 randomPos;

        do
        {
            // 巡回範囲を半径とする球内の、ランダムな地点を取得する
            randomPos = Random.insideUnitSphere * _patrolRadius;

            // ランダムな地点のY座標を0に設定する
            randomPos.y = 0;

            // ランダムな地点は原点を中心としているため、キャラクターの位置を加算する
            randomPos += transform.position;

            // ランダムな地点への方向を求める
            Vector3 directionToRandomPos = (randomPos - transform.position).normalized;

            // 現在の方向(ロボットの前方)とランダムな地点への方向との間の角度を求める
            rotationAngle = Vector3.Angle(transform.forward, directionToRandomPos);
        }
        // 回転距離が最小回転角度を上回るまで繰り返す
        while (rotationAngle < MIN_ANGLE);

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
            // 目標地点への方向を求める
            var dir = (_patrolDestination.Value - transform.position).normalized;

            // 求めた方向を基に、目標地点への回転方向を求める
            Quaternion lookRotation = Quaternion.LookRotation(dir);

            // 目標地点へ回転中であることを示すフラグをオンにする
            _isRotating = true;

            // 目標地点の方向へ回転させる
            Tween rotationTween = transform.DORotate(lookRotation.eulerAngles, ROTATION_DURATION);

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

    /// <summary>衝突判定を行う</summary>
    /// <returns>true = 衝突が発生した、　false = 衝突が発生しなかった</returns>
    private bool IsCollided()
    {
        // レイキャストの衝突情報を格納する変数
        RaycastHit hit;

        // キャラクターの現在の位置を取得
        Vector3 currentPos = transform.position;

        // レイキャストを飛ばす位置を設定
        Vector3 raycastPos = new Vector3(currentPos.x, RAYCAST_Y_OFFSET, currentPos.z);

        // レイキャストを前方に飛ばし、衝突が発生した場合はtrueを返す
        if (Physics.Raycast(raycastPos, transform.forward, out hit, RAYCAST_DISTANCE))
        {
            // 接触したオブジェクトが存在する場合
            if (hit.collider)
            {
                return true;
            }
        }

        // 衝突が発生しなかった場合はfalseを返す
        return false;
    }

    /// <summary>衝突判定をチェックし、目標地点をクリアする</summary>
    private void CheckCollisionThenClearDestination()
    {
        // 衝突が発生している場合
        if (IsCollided())
        {
            // 目標地点をクリアする
            _patrolDestination = null;
        }
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
        Gizmos.DrawRay(raycastPos, transform.forward * RAYCAST_DISTANCE);
    }
}
