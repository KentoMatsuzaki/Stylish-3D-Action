using UnityEngine;
using DG.Tweening;
using System.Collections;

/// <summary>空陸両用ロボット</summary>
public class Robot : MonoBehaviour
{ 
    [SerializeField, Header("巡回アクションの設定項目")] private PatrolSettings _patrolSettings;

    [SerializeField, Header("追跡アクションの設定項目")] private ChaseSettings _chaseSettings;

    [SerializeField, Header("攻撃アクションの設定項目")] private AttackSettings _attackSettings;

    [SerializeField, Header("敵の体力")] private float _hp;

    [SerializeField, Header("ノックバック強度")] private float _knockbackForce;

    [SerializeField, Header("攻撃エフェクトを生成する位置")] private Transform _muzzle;

    /// <summary>初期化アニメーションの完了を示すフラグ</summary>
    [SerializeField, Header("初期化フラグ")] private bool _isInitialized = false;

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

    public float PlayerDetectionRange => _chaseSettings._playerDetectionRange;

    /// <summary>攻撃フラグ</summary>
    private bool _isAttacking = false;

    /// <summary>死亡フラグ</summary>
    private bool _isDead = false;

    /// <summary>ノックバック処理のシーケンス</summary>
    private Sequence _knockbackSequence;

    /// <summary>ルートノード</summary>
    private BaseNode _rootNode;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _rootNode = ConstructBehaviorTree();
    }

    private void Update()
    {
        // 初期化アニメーションの再生が終了していない場合は、処理を抜ける
        if (!_isInitialized) return;

        if (_isDead) return;

        if (_isAttacking) return;

        if (_rootNode != null) _rootNode.Execute();
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

    /// <summary>プレイヤーを感知していない</summary>
    private bool HasPlayerNotDetected()
    {
        return GetHorizontalDistanceToPlayer() > _chaseSettings._playerDetectionRange;
    }

    /// <summary>到達判定距離よりも遠くにプレイヤーを感知している</summary>
    private bool HasPlayerDetectedWithDistance()
    {
        if ((GetHorizontalDistanceToPlayer() <= _chaseSettings._playerDetectionRange) &&
            !HasReachedPlayer()) return true;
        else return false;
    }

    /// <summary>到達判定距離よりも近くにプレイヤーを感知している</summary>
    private bool HasPlayerDetectedWithoutDistance()
    {
        return HasReachedPlayer();
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

    /// <summary>プレイヤーを追跡する</summary>
    /// <returns>追跡アクションノードの評価結果</returns>
    private NodeStatus Chase()
    {
        // 巡回目的地をクリアする
        ClearPatrolDestination();

        // 追跡対象の元に到達した場合、成功の評価結果を返す
        if (HasReachedPlayer()) return NodeStatus.Success;

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
    private bool HasReachedPlayer()
    {
        return GetDistanceToPlayer() <= _chaseSettings._chaseArrivalThreshold;
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
            _chaseSettings._chaseRotationSlerpSpeed * Time.deltaTime);
    }

    //-------------------------------------------------------------------------------
    // 攻撃
    //-------------------------------------------------------------------------------

    /// <summary>プレイヤーを攻撃する</summary>
    /// <returns>攻撃アクションノードの評価結果</returns>
    private NodeStatus Attack()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, GetHorizontalRotationToPlayer(), 1.0f);

        // 攻撃フラグをオンにする
        _isAttacking = true;

        // 攻撃トリガーをオンにする
        _animator.SetTrigger(AttackSettings.ATTACK_TRIGGER);

        // 攻撃が一度だけ実行されることが保証されているため、成功の評価結果を返す
        return NodeStatus.Success;
    }

    //-------------------------------------------------------------------------------
    // 攻撃に関する処理
    //-------------------------------------------------------------------------------

    /// <summary>攻撃アニメーションの再生を完了した際に呼ばれるコールバックメソッド</summary>
    /// <summary>攻撃アニメーションイベントから呼ばれる</summary>
    public void OnCompleteAttack()
    {
        // 攻撃フラグをオフにする
        _isAttacking = false;
    }

    /// <summary>攻撃エフェクトを生成する</summary>
    /// <param name="attackEffectIndex">攻撃エフェクトのインデックス</param>
    public void CreateAttackEffect(int attackEffectIndex)
    {
        // 攻撃エフェクトを生成する
        GameObject attackEffect =  EffectManager.Instance.CreateEnemyAttackEffect(attackEffectIndex, 
            _muzzle.position);

        // 攻撃エフェクトの攻撃力を自身の攻撃力に設定する
        attackEffect.GetComponent<EnemyAttacker>().Power = _attackSettings._power;

        // 攻撃エフェクトの回転を自身の回転と同期させる
        attackEffect.transform.rotation = transform.rotation;
    }

    //-------------------------------------------------------------------------------
    // 被ダメージ
    //-------------------------------------------------------------------------------

    /// <summary>被ダメージ処理</summary>
    private void GetHit()
    {
        // 被ダメージアニメーションを再生
        _animator.Play("Get Hit");

        // ノックバック処理
        ApplyKnockback();

        // 敵の攻撃が中断された場合に攻撃のキャンセル処理を行う
        OnCompleteAttack();
    }

    //-------------------------------------------------------------------------------
    // 被ダメージに関する処理
    //-------------------------------------------------------------------------------

    /// <summary>衝突判定</summary>
    private void OnTriggerEnter(Collider collision)
    {
        // プレイヤーの攻撃判定との衝突ではない場合、処理を抜ける
        if (!collision.gameObject.CompareTag("PlayerAttack")) return;

        if (_isDead) return;

        IAttacker attacker;

        // ダメージ処理
        if (collision.gameObject.TryGetComponent<IAttacker>(out attacker))
        {
            int damage = attacker.Power;
            TakeDamage(damage);
        }

        if (_isDead) return;

        EffectManager.Instance.CreateAttackHitEffect(0, collision.ClosestPointOnBounds(transform.position));
        GetHit();
    }

    /// <summary>ダメージ適用処理</summary>
    private void TakeDamage(int damage)
    {
        // ダメージを適用
        _hp -= damage;

        // 死亡している場合は、死亡処理を実行する
        if (IsDead())
        {
            Debug.Log("Dead");
            _animator.Play("Die");
            _isDead = true;
            Spawner.Instance.DecreaseEnemyCount();
            GameManager.Instance.AddComboCount();
        }
    }

    /// <summary>死亡判定</summary>
    private bool IsDead()
    {
        return _hp <= 0 ? true : false;
    }

    /// <summary>ノックバック処理</summary>
    private void ApplyKnockback()
    {
        // 前回のノックバックシーケンスを終了させる
        _knockbackSequence?.Kill();

        // ノックバックさせる方向を取得する
        var current = transform.position;
        var playerPos = Player.Instance.transform.position;

        current.y = 0;
        playerPos.y = 0;

        var knockbackDir = (current - playerPos).normalized;

        // ノックバックの移動目標座標
        var endPos = transform.position + knockbackDir * _knockbackForce;

        _knockbackSequence = DOTween.Sequence().SetLink(gameObject).
            Append(transform.DOMove(endPos, 0.5f).SetEase(Ease.OutCubic));
    }

    /// <summary>自身のゲームオブジェクトを破棄する</summary>
    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    /// <summary>死亡時の処理を完了した際に呼ばれるメソッド</summary>
    /// <summary>アニメーションイベントから呼ばれる</summary>
    public void OnCompletedDeath()
    {
        var currentPos = transform.position;
        currentPos.y += 1f;
        EffectManager.Instance.CreateDeathEffect(0, currentPos);
        Invoke(nameof(DestroySelf), 0.1f);
    }

    //-------------------------------------------------------------------------------
    // BehaviorTreeに関連しない処理
    //-------------------------------------------------------------------------------

    /// <summary>ビヘイビアツリーの構築</summary>
    private BaseNode ConstructBehaviorTree()
    {
        // アクションノードの登録
        var patrolAction = new ActionNode(() => Patrol()); // 巡回
        var chaseAction = new ActionNode(() => Chase()); // 追跡
        var attackAction = new ActionNode(() => Attack()); // 攻撃

        // 条件ノードの登録
        var hasPlayerNotDetected = new ConditionNode(() => HasPlayerNotDetected());
        var hasPlayerDetectedWithDistance = new ConditionNode(() => HasPlayerDetectedWithDistance());
        var hasPlayerDetectedWithoutDistance = new ConditionNode(() => HasPlayerDetectedWithoutDistance());

        // 巡回シーケンスの登録
        var patrolSequence = new SequenceNode();
        patrolSequence.AddChild(hasPlayerNotDetected);
        patrolSequence.AddChild(patrolAction);

        // 追跡シーケンスの登録
        var chaseSequence = new SequenceNode();
        chaseSequence.AddChild(hasPlayerDetectedWithDistance);
        chaseSequence.AddChild(chaseAction);

        // 攻撃シーケンスの登録
        var attackSequence = new SequenceNode();
        attackSequence.AddChild(hasPlayerDetectedWithoutDistance);
        attackSequence.AddChild(attackAction);

        // セレクターノードの登録
        var rootSelector = new SelectorNode();
        rootSelector.AddChild(patrolSequence);
        rootSelector.AddChild(chaseSequence);
        rootSelector.AddChild(attackSequence);

        return rootSelector;
    }
}