using UnityEngine;

/// <summary>必殺技エフェクト</summary>
public class UltEffect : AttackEffectBase
{
    protected override void Start()
    {
        // 基底クラスのStartメソッドを呼び出す
        base.Start();
    }

    /// <summary>必殺技エフェクト固有の処理</summary>
    protected override void UniqueAction()
    {
        
    }
}
