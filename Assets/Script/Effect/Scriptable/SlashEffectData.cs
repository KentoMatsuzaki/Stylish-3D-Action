using UnityEngine;

/// <summary>斬撃エフェクトのデータをまとめたクラス</summary>
[CreateAssetMenu(fileName = "SlashEffectData", menuName = "ScriptableObjects/SlashEffectData")]

public class SlashEffectData : ScriptableObject
{
    /// <summary>斬撃エフェクト(水平・右)</summary>
    [SerializeField, Header("斬撃エフェクト(水平・右)")] public GameObject _horizontalRightSlashEffect;

    /// <summary>斬撃エフェクト(水平・左)</summary>
    [SerializeField, Header("斬撃エフェクト(水平・左)")] public GameObject _horizontalLeftSlashEffect;

    /// <summary>斬撃エフェクト(斬り上げ・右)</summary>
    [SerializeField, Header("斬撃エフェクト(斬り上げ・右)")] public GameObject _lowerRightSlashEffect;

    /// <summary>斬撃エフェクト(斬り上げ・左)</summary>
    [SerializeField, Header("斬撃エフェクト(斬り上げ・左)")] public GameObject _lowerLeftSlashEffect;

    /// <summary>斬撃エフェクト(斬り下げ・右)</summary>
    [SerializeField, Header("斬撃エフェクト(斬り下げ・右)")] public GameObject _upperRightSlashEffect;

    /// <summary>斬撃エフェクト(斬り下げ・左)</summary>
    [SerializeField, Header("斬撃エフェクト(斬り下げ・左)")] public GameObject _upperLeftSlashEffect;
}