using UnityEngine;

/// <summary>斬撃エフェクト</summary>
public class SlashEffect : AttackEffectBase
{
    [SerializeField] bool _isRightEffect = true;
    protected override void Start()
    {
        // 基底クラスのStartメソッドを呼び出す
        base.Start();

        UniqueAction();
    }

    /// <summary>斬撃エフェクト固有の処理</summary>
    protected override void UniqueAction()
    {
        if (_isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, 180f);
        }
    }
}
