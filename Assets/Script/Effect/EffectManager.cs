using UnityEngine;
using System.Collections.Generic;

/// <summary>�e��G�t�F�N�g���Ǘ�����N���X</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>�a���G�t�F�N�g�i�E�j�̃��X�g</summary>
    [SerializeField, Header("�a���G�t�F�N�g�i�E�j")] 
        private List<GameObject> _rightSlashEffectList = new List<GameObject>();

    /// <summary>�a���G�t�F�N�g�i�E�j�̑����ƃC���f�b�N�X�̃}�b�v</summary>
    private Dictionary<AttackEffectType, int> _rightSlashEffectIndexMap;

    /// <summary>�a���G�t�F�N�g�i���j�̃��X�g</summary>
    [SerializeField, Header("�a���G�t�F�N�g�i���j")]
        private List<GameObject> _leftSlashEffectList = new List<GameObject>();

    /// <summary>�a���G�t�F�N�g�i���j�̑����ƃC���f�b�N�X�̃}�b�v</summary>
    private Dictionary<AttackEffectType, int> _leftSlashEffectIndexMap;

    /// <summary>�K�E�Z�G�t�F�N�g�̃��X�g</summary>
    [SerializeField, Header("�K�E�Z�G�t�F�N�g")] 
        private List<GameObject> _ultEffectList = new List<GameObject>();

    /// <summary>�K�E�Z�G�t�F�N�g�̏��ƃC���f�b�N�X�̃}�b�v</summary>
    private Dictionary<AttackEffectType, int> _ultEffectIndexMap;

    protected override void Awake()
    {
        // �V���O���g���̐ݒ�
        base.Awake();

        // �a���G�t�F�N�g�i�E�j�̃}�b�v�̏�����
        _rightSlashEffectIndexMap = new Dictionary<AttackEffectType, int>
        {
            {AttackEffectType.Ink, 0},
            {AttackEffectType.RedFlame, 1},
            {AttackEffectType.BlueFlame, 2},
            {AttackEffectType.Nebula, 3},
            {AttackEffectType.Blood, 4},
            {AttackEffectType.Water, 5}
        };

        // �a���G�t�F�N�g�i���j�̃}�b�v�̏�����
        _leftSlashEffectIndexMap = new Dictionary<AttackEffectType, int>
        {
            {AttackEffectType.Ink, 0},
            {AttackEffectType.RedFlame, 1},
            {AttackEffectType.BlueFlame, 2},
            {AttackEffectType.Nebula, 3},
            {AttackEffectType.Blood, 4},
            {AttackEffectType.Water, 5}
        };

        // �K�E�Z�G�t�F�N�g�̃}�b�v�̏�����
        _ultEffectIndexMap = new Dictionary<AttackEffectType, int>
        {
            {AttackEffectType.Wind , 0},
            {AttackEffectType.Lightning, 1},
            {AttackEffectType.White, 2}
        };
    }

    /// <summary>�a���G�t�F�N�g�i�E�j�𐶐��E�\������</summary>
    /// <param name="type">�a���G�t�F�N�g�̑���</param>
    /// <param name="pos">�a���G�t�F�N�g�̐����ʒu</param>
    public void PlayRightSlashEffect(AttackEffectType type, Vector3 pos)
    { 
        if (!_rightSlashEffectIndexMap.TryGetValue((type), out int index))
        {
            // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��蓖�Ă��Ă��邽�߁j
            index = -1;
        }

        // index �̒l���L�����ǂ������`�F�b�N
        if (index >= 0 && index < _rightSlashEffectList.Count)
        {
            Instantiate(_rightSlashEffectList[index], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect type not found or Index is out of range.");
        }
    }

    /// <summary>�a���G�t�F�N�g�i���j�𐶐��E�\������</summary>
    /// <param name="type">�a���G�t�F�N�g�̑���</param>
    /// <param name="pos">�a���G�t�F�N�g�̐����ʒu</param>
    public void PlayLeftSlashEffect(AttackEffectType type, Vector3 pos)
    {
        if (!_leftSlashEffectIndexMap.TryGetValue((type), out int index))
        {
            // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��蓖�Ă��Ă��邽�߁j
            index = -1;
        }

        // index �̒l���L�����ǂ������`�F�b�N
        if (index >= 0 && index < _leftSlashEffectList.Count)
        {
            Instantiate(_leftSlashEffectList[index], pos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Effect type not found or Index is out of range.");
        }
    }

    /// <summary>�K�E�Z�G�t�F�N�g�𐶐��E�\������</summary>
    /// <param name="type">�K�E�Z�G�t�F�N�g�̑���</param>
    /// <param name="pos">�K�E�Z�G�t�F�N�g�̐����ʒu</param>
    public void DisplayUltEffect(AttackEffectType type, Vector3 pos)
    {
        if (!_ultEffectIndexMap.TryGetValue((type), out int index))
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
            Debug.LogWarning($"Effect type not found or Index is out of range.");
        }
    }
}
