using UnityEngine;

/// <summary>ロボットの攻撃アクションに関する設定項目</summary>
[CreateAssetMenu(fileName = "AttackSettings", menuName = "ScriptableObjects/AttackSettings")]

public class AttackSettings : ScriptableObject
{
    /// <summary>攻撃力</summary>
    public int _power;

    /// <summary>攻撃トリガー</summary>
    public const string ATTACK_TRIGGER = "Attack";

    /// <summary>攻撃エフェクトの生成位置のY座標オフセット</summary>
    public float _attackEffectPositionOffsetY;

    /// <summary>攻撃エフェクトの生成位置のY座標オフセット</summary>
    public float _attackEffectPositionOffsetZ;
}
