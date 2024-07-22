using UnityEngine;
using System.Collections.Generic;

/// <summary>�e��G�t�F�N�g���Ǘ�����N���X</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>�a���G�t�F�N�g�̃��X�g</summary>
    [SerializeField, Header("�a���G�t�F�N�g")] private List<GameObject> _slashEffectList = new List<GameObject>();

    /// <summary>�a���G�t�F�N�g�̏��ƃC���f�b�N�X�̃}�b�v</summary>
    private Dictionary<(AttackEffectType, AttackEffectCategory), int> _slashEffectIndexMap;

    /// <summary>�K�E�Z�G�t�F�N�g�̃��X�g</summary>
    [SerializeField, Header("�a���G�t�F�N�g")] private List<GameObject> _ultEffectList = new List<GameObject>();

    /// <summary>�K�E�Z�G�t�F�N�g�̏��ƃC���f�b�N�X�̃}�b�v</summary>
    private Dictionary<(AttackEffectType, AttackEffectCategory), int> _ultEffectIndexMap;

    protected override void Awake()
    {
        // �V���O���g���̐ݒ�
        base.Awake();

        // �a���G�t�F�N�g�̃}�b�v�̏�����
        _slashEffectIndexMap = new Dictionary<(AttackEffectType, AttackEffectCategory), int>
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
        };

        // �K�E�Z�G�t�F�N�g�̃}�b�v�̏�����
        _ultEffectIndexMap = new Dictionary<(AttackEffectType, AttackEffectCategory), int>
        {
            {(AttackEffectType.Wind, AttackEffectCategory.Ult), 12},
            {(AttackEffectType.Lightning, AttackEffectCategory.Ult), 13},
            {(AttackEffectType.White, AttackEffectCategory.Ult), 14}
        };
    }

    /// <summary>�a���G�t�F�N�g�𐶐��E�\������</summary>
    /// <param name="type">�a���G�t�F�N�g�̑���</param>
    /// <param name="category">�a���G�t�F�N�g�̕���</param>
    /// <param name="pos">�a���G�t�F�N�g�̐����ʒu</param>
    public void DisplaySlashEffect(AttackEffectType type, AttackEffectCategory category, Vector3 pos)
    { 
        if (!_slashEffectIndexMap.TryGetValue((type, category), out int index))
        {
            // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��蓖�Ă��Ă��邽�߁j
            index = -1;
        }

        // index �̒l���L�����ǂ������`�F�b�N
        if (index >= 0 && index < _slashEffectList.Count)
        {
            Instantiate(_slashEffectList[index], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect type or category not found or Index is out of range.");
        }
    }

    /// <summary>�K�E�Z�G�t�F�N�g�𐶐��E�\������</summary>
    /// <param name="type">�K�E�Z�G�t�F�N�g�̑���</param>
    /// <param name="category">�K�E�Z�G�t�F�N�g�̕���</param>
    /// <param name="pos">�K�E�Z�G�t�F�N�g�̐����ʒu</param>
    public void DisplayUltEffect(AttackEffectType type, AttackEffectCategory category, Vector3 pos)
    {
        if (!_ultEffectIndexMap.TryGetValue((type, category), out int index))
        {
            // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��蓖�Ă��Ă��邽�߁j
            index = -1;
        }

        // index �̒l���L�����ǂ������`�F�b�N
        if (index >= 0 && index < _ultEffectList.Count)
        {
            Instantiate(_ultEffectList[index], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect type or category not found or Index is out of range.");
        }
    }
}
