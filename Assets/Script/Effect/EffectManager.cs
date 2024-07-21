using UnityEngine;
using System.Collections.Generic;

/// <summary>�e��G�t�F�N�g���Ǘ�����N���X</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>�U���G�t�F�N�g�̃v���n�u�̃��X�g</summary>
    [SerializeField, Header("�U���G�t�F�N�g")] private List<GameObject> _attackEffectList = new List<GameObject>();

    /// <summary>�U���G�t�F�N�g�̏��ƑΉ�����C���f�b�N�X�̃}�b�v</summary>
    private Dictionary<(AttackEffectType, AttackEffectCategory), int> _attackEffectIndexMap;

    protected override void Awake()
    {
        // �V���O���g���̐ݒ�
        base.Awake();

        // �}�b�v�̏�����
        _attackEffectIndexMap = new Dictionary<(AttackEffectType, AttackEffectCategory), int>
        {
            {(AttackEffectType.Ink, AttackEffectCategory.Right), 0},
            {(AttackEffectType.Ink, AttackEffectCategory.Left), 1},
            {(AttackEffectType.BlueFlame, AttackEffectCategory.Right), 2},
            {(AttackEffectType.BlueFlame, AttackEffectCategory.Left), 3},
            {(AttackEffectType.RedFlame, AttackEffectCategory.Right), 4},
            {(AttackEffectType.RedFlame, AttackEffectCategory.Left), 5},
            {(AttackEffectType.Nebula, AttackEffectCategory.Right), 6},
            {(AttackEffectType.Nebula, AttackEffectCategory.Left), 7},
            {(AttackEffectType.Blood, AttackEffectCategory.Right), 8},
            {(AttackEffectType.Blood, AttackEffectCategory.Left), 9},
            {(AttackEffectType.Water, AttackEffectCategory.Right), 10},
            {(AttackEffectType.Water, AttackEffectCategory.Left), 11},
            {(AttackEffectType.Wind, AttackEffectCategory.Ult), 12},
            {(AttackEffectType.Lightning, AttackEffectCategory.Ult), 13},
            {(AttackEffectType.White, AttackEffectCategory.Ult), 14}
        };
    }

    /// <summary>�U���G�t�F�N�g��\������</summary>
    /// <param name="pos">�U���G�t�F�N�g�̐����ʒu</param>
    public void DisplayAttackEffect(AttackEffectType type, AttackEffectCategory category, Vector3 pos)
    { 
        if (!_attackEffectIndexMap.TryGetValue((type, category), out int index))
        {
            // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��蓖�Ă��Ă��邽�߁j
            index = -1;
        }

        // index �̒l���L�����ǂ������`�F�b�N
        if (index >= 0 && index < _attackEffectList.Count)
        {
            Instantiate(_attackEffectList[index], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect type or category not found or Index is out of range.");
        }
    }
}
