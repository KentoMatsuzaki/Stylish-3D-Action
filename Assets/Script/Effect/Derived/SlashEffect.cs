using UnityEngine;

/// <summary>斬撃エフェクト</summary>
public class SlashEffect : AttackEffectBase
{
    /// <summary>右方向のエフェクトかどうか</summary>
    [SerializeField] bool _isRightEffect = true;

    /// <summary>斬撃エフェクト固有の処理</summary>
    protected override void UniqueAction()
    {
        if (_isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 180f);
        }
    }
}
