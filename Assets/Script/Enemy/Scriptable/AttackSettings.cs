using UnityEngine;

/// <summary>���{�b�g�̍U���A�N�V�����Ɋւ���ݒ荀��</summary>
[CreateAssetMenu(fileName = "AttackSettings", menuName = "ScriptableObjects/AttackSettings")]

public class AttackSettings : ScriptableObject
{
    /// <summary>�U����</summary>
    public int _power;

    /// <summary>�U���g���K�[</summary>
    public const string ATTACK_TRIGGER = "Attack";
}
