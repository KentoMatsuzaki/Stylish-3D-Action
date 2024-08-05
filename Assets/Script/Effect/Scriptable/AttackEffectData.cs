using UnityEngine;

/// <summary>攻撃エフェクトのデータをまとめたクラス</summary>
[CreateAssetMenu(fileName = "AttackEffectData", menuName = "ScriptableObjects/AttackEffectData")]

public class AttackEffectData : ScriptableObject
{
    /// <summary>斬撃エフェクト(右)</summary>
    [SerializeField, Header("斬撃エフェクト(右)")] public GameObject _rightSlashEffect;

    /// <summary>斬撃エフェクト(左)</summary>
    [SerializeField, Header("斬撃エフェクト(左)")] public GameObject _leftSlashEffect;

    /// <summary>特殊エフェクト(斬り上げ・右)</summary>
    [SerializeField, Header("特殊エフェクト(斬り上げ・右)")] public GameObject _lowerRightAltEffect;

    /// <summary>特殊エフェクト(斬り上げ・左)</summary>
    [SerializeField, Header("特殊エフェクト(斬り上げ・左)")] public GameObject _lowerLeftAltEffect;

    /// <summary>特殊エフェクト(斬り下げ・右)</summary>
    [SerializeField, Header("特殊エフェクト(斬り下げ・右)")] public GameObject _upperRightAltEffect;

    /// <summary>特殊エフェクト(斬り下げ・左)</summary>
    [SerializeField, Header("特殊エフェクト(斬り下げ・左)")] public GameObject _upperLeftAltEffect;
}