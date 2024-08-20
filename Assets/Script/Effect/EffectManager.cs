using UnityEngine;
using System.Collections.Generic;

/// <summary>�e��G�t�F�N�g���Ǘ�����N���X</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>�a���G�t�F�N�g�𐶐�����ۂ̍����̃I�t�Z�b�g</summary>
    [SerializeField] private float _slashEffectHeightOffset = 1.25f;

    /// <summary>�a���G�t�F�N�g�f�[�^�̃��X�g</summary>
    [SerializeField, Header("�a���G�t�F�N�g�f�[�^�̃��X�g")]
        private List<SlashEffectData> _slashEffectDataList = new List<SlashEffectData>();

    //-------------------------------------------------------------------------------
    // �a���G�t�F�N�g�𐶐����鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�����̎a���G�t�F�N�g�𐶐����鋤�ʃ��\�b�h</summary>
    public void CreateHorizontalSlashEffect(AttackHand hand)
    {
        var effects = GetSlashEffect(SlashType.Horizontal, hand);

        effects.ForEach(effect => CreateEffect(effect, GetSlashEffectPosition()));
    }

    /// <summary>�a��グ�̎a���G�t�F�N�g�𐶐����鋤�ʃ��\�b�h</summary>
    public void CreateLowerSlashEffect(AttackHand hand)
    {
        var effects = GetSlashEffect(SlashType.Lower, hand);

        effects.ForEach(effect => CreateEffect(effect, GetSlashEffectPosition()));
    }

    /// <summary>�a�艺���̎a���G�t�F�N�g�𐶐����鋤�ʃ��\�b�h</summary>
    public void CreateUpperSlashEffect(AttackHand hand)
    {
        var effects = GetSlashEffect(SlashType.Upper, hand);

        effects.ForEach(effect => CreateEffect(effect, GetSlashEffectPosition()));
    }

    //-------------------------------------------------------------------------------
    // �a���G�t�F�N�g�̐����Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�G�t�F�N�g�𐶐����鋤�ʃ��\�b�h</summary>
    /// <param name="effectPrefab">�G�t�F�N�g�̃v���n�u</param>
    /// <param name="position">�����ʒu</param>
    private void CreateEffect(GameObject effectPrefab, Vector3 position)
    {
        Instantiate(effectPrefab, position, Quaternion.identity, transform);
    }

    /// <summary>�a���G�t�F�N�g�𐶐�����ʒu�����߂�</summary>
    private Vector3 GetSlashEffectPosition()
    {
        // �v���C���[�̈ʒu�ɃI�t�Z�b�g���������ʒu�����߂ĕԂ�
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 effectPos = new Vector3(playerPos.x, playerPos.y + _slashEffectHeightOffset, playerPos.z);
        return effectPos;
    }

    /// <summary>�a���G�t�F�N�g���擾����</summary>
    /// <returns>�a���G�t�F�N�g�̃v���n�u�̃��X�g</returns>
    private List<GameObject> GetSlashEffect(SlashType type, AttackHand hand)
    {
        // �v���C���[�����a���̑�������ɁA�a���G�t�F�N�g�̃f�[�^���擾����
        var data = _slashEffectDataList[(int)Player.Instance.Enchantment];

        // �a���G�t�F�N�g���i�[���郊�X�g���쐬����
        var effects = new List<GameObject>();

        switch(type)
        {
            case SlashType.Horizontal:

                if(IsRightHandSlash(hand)) effects.Add(data._horizontalRightSlashEffect);
                if(IsLeftHandSlash(hand)) effects.Add(data._horizontalLeftSlashEffect); break;

            case SlashType.Lower:

                if(IsRightHandSlash(hand)) effects.Add(data._lowerRightSlashEffect);
                if(IsLeftHandSlash(hand)) effects.Add(data._lowerLeftSlashEffect); break;

            case SlashType.Upper:

                if(IsRightHandSlash(hand)) effects.Add(data._upperRightSlashEffect);
                if(IsLeftHandSlash(hand)) effects.Add(data._upperLeftSlashEffect); break;   
        }

        return effects;
    }

    /// <summary>�E��ɂ��a���ł���ꍇ��true��Ԃ�</summary>
    /// <returns>true = �E��E����Cfalse = ����</returns>
    private bool IsRightHandSlash(AttackHand hand)
    {
        if(hand == AttackHand.Right ||  hand == AttackHand.Both) return true;
        else return false;
    }

    /// <summary>����ɂ��a���ł���ꍇ��true��Ԃ�</summary>
    /// <returns>true = ����E����, false = �E��</returns>
    private bool IsLeftHandSlash(AttackHand hand)
    {
        if (hand == AttackHand.Left || hand == AttackHand.Both) return true;
        else return false;
    }
}