using UnityEngine;

/// <summary>斬撃エフェクトのデータをまとめたクラス</summary>
[CreateAssetMenu(fileName = "SlashEffectData", menuName = "ScriptableObjects/SlashEffectData")]

public class SlashEffectData : ScriptableObject
{
    /// <summary>斬撃の付加効果</summary>
    public SlashEnchantment _enchantment;

    /// <summary>斬撃エフェクト(右)</summary>
    public GameObject _rightSlashEffect;

    /// <summary>斬撃エフェクト(左)</summary>
    public GameObject _leftSlashEffect;
}
