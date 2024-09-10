using DG.Tweening;
using UnityEngine;

/// <summary>特殊攻撃クラス</summary>
public class AltEffectAttacker : MonoBehaviour, IAttacker
{
    /// <summary>プレイヤーの攻撃力</summary>
    [SerializeField, Header("攻撃力")] private int _power;

    /// <summary>敵のゲームオブジェクトに付されるタグ</summary>
    private const string enemyTag = "Enemy";
    
    /// <summary>特殊攻撃エフェクトを移動させるTween</summary>
    private Tween _moveTween;
    
    /// <summary>特殊攻撃エフェクトの移動速度</summary>
    private const float MOVE_SPEED = 7.5f;

    /// <summary>特殊攻撃エフェクトの移動時間</summary>
    private const float MOVE_DURATION = 1f;

    /// <summary>攻撃力のプロパティ</summary>
    public int Power
    {
        get => _power;
        set => _power = value > 0 ? value : 0;
    }

    void Start()
    {
        MoveForwardIndefinitely();
    }

    /// <summary>他のコライダーに接触した際に呼ばれる処理</summary>
    public void OnTriggerEnter(Collider other)
    {
        // 敵に接触した場合
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Hit");

            // Tweenを停止させる
            _moveTween.Kill();

            // エフェクトのゲームオブジェクトを破棄する
            Destroy(gameObject);
        }
    }

    /// <summary>エフェクトを無制限に前方へ移動させる</summary>
    private void MoveForwardIndefinitely()
    {
        // エフェクトが存在する場合は、無制限に前方へ移動させる
        _moveTween = transform.DOMove(transform.position + -transform.forward * MOVE_SPEED, MOVE_DURATION)
            .SetEase(Ease.Linear)
            .OnComplete(() => MoveForwardIndefinitely());
    }
}
