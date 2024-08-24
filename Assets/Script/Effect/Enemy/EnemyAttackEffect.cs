using UnityEngine;

/// <summary>敵の攻撃エフェクトにアタッチするクラス</summary>
public class EnemyAttackEffect : MonoBehaviour
{
    /// <summary>エフェクトの種類</summary>
    public enum EnemyAttackEffectType
    {
        /// <summary>直進</summary>
        Straight,

        /// <summary>追尾</summary>
        Homing
    }

    /// <summary>攻撃エフェクトの種類</summary>
    [SerializeField, Header("攻撃エフェクトの種類")] private EnemyAttackEffectType _effectType;

    /// <summary>攻撃エフェクトの移動速度</summary>
    [SerializeField, Header("攻撃エフェクトの移動速度")] private float _moveSpeed;

    /// <summary>追尾時の回転補完係数</summary>
    [SerializeField, Header("追尾時の回転補完係数")] private float _rotationSlerpSpeed;

    private void Update()
    {
        switch(_effectType)
        {
            // 直進するエフェクト
            case EnemyAttackEffectType.Straight:
                MoveForward(); break;

            // 追尾するエフェクト
            case EnemyAttackEffectType.Homing:
                HomingPlayer(); break;
        }
    }

    /// <summary>前方に移動する</summary>
    private void MoveForward()
    {
        transform.Translate(transform.forward * _moveSpeed * Time.deltaTime);
    }

    /// <summary>プレイヤーを追尾する</summary>
    private void HomingPlayer()
    {
        // プレイヤーの方向へ回転する
        RotateTowardsPlayer();

        // 前方に移動する
        MoveForward();
    }

    /// <summary>プレイヤーへのY座標を無視した方向を求める</summary>
    private Vector3 GetHorizontalDirectionToPlayer()
    {
        Vector3 dir = (Player.Instance.transform.position - transform.position).normalized;
        Vector3 fixedDir = new Vector3(dir.x, 0, dir.z);
        return fixedDir;
    }

    /// <summary>プレイヤーへのY座標を無視した回転を求める</summary>
    private Quaternion GetHorizontalRotationToPlayer()
    {
        return Quaternion.LookRotation(GetHorizontalDirectionToPlayer());
    }

    /// <summary>プレイヤーの方向へ回転する</summary>
    private void RotateTowardsPlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, GetHorizontalRotationToPlayer(),
            _rotationSlerpSpeed * Time.deltaTime);
    }
}
