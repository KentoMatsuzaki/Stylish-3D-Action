using UnityEngine;

/// <summary>���{�b�g�̍U���A�N�V�����Ɋւ���ݒ荀��</summary>
[CreateAssetMenu(fileName = "AttackSettings", menuName = "ScriptableObjects/AttackSettings")]

public class AttackSettings : ScriptableObject
{
    /// <summary>�U����</summary>
    public int _power;

    /// <summary>�U���g���K�[</summary>
    public const string ATTACK_TRIGGER = "Attack";

    /// <summary>�U���G�t�F�N�g�̐����ʒu��Y���W�I�t�Z�b�g</summary>
    public float _attackEffectPositionOffsetY;   
}
