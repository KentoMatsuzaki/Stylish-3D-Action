using UnityEngine;

/// <summary>特殊エフェクト(斬り上げ)のデータをまとめたクラス</summary>
[CreateAssetMenu(fileName = "LowerAltEffectData", menuName = "ScriptableObjects/LowerAltEffectData")]

public class LowerAltEffectData : ScriptableObject
{
    /// <summary>斬撃の付加効果</summary>
    public SlashEnchantment _enchantment;

    /// <summary>特殊エフェクト(右)</summary>
    public GameObject _rightAltEffect;

    /// <summary>特殊エフェクト(左)</summary>
    public GameObject _leftAltEffect;
}
