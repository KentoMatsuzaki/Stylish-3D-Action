using UnityEngine;

/// <summary>特殊エフェクトのデータをまとめたクラス</summary>
[CreateAssetMenu(fileName = "AltEffectData", menuName = "ScriptableObjects/AltEffectData")]

public class AltEffectData : ScriptableObject
{
    /// <summary>斬撃の付加効果</summary>
    public SlashEnchantment _enchantment;

    /// <summary>特殊エフェクト(右)</summary>
    public GameObject _rightAltEffect;

    /// <summary>特殊エフェクト(左)</summary>
    public GameObject _leftAltEffect;

}
