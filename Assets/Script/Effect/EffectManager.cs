using UnityEngine;
using System.Collections.Generic;

/// <summary>�e��G�t�F�N�g���Ǘ�����N���X</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>�U���G�t�F�N�g�f�[�^�̃��X�g</summary>
    [SerializeField, Header("�U���G�t�F�N�g�f�[�^�̃��X�g")]
        private List<AttackEffectData> _attackEffectList = new List<AttackEffectData>();

    /// <summary>�a���G�t�F�N�g�𐶐����ĕ\�����郁�\�b�h</summary>
    /// <param name="position">��������ʒu</param>
    /// <param name="handIndex">�U���ɗp�����������C���f�b�N�X�i0���E��A1������A2������j</param>
    public void CreateSlashEffect(Vector3 position, int handIndex)
    {
        Instantiate(_attackEffectList[(int)Player.Instance.Enchantment])

        // index�̒l������ł���ꍇ
        if (index >= 0 && index < _slashEffectList.Count)
        {
            switch (handIndex)
            {
                // �E��
                case 0:
                    // �E�����̃G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var rightEffect = Instantiate(_slashEffectList[index], position, Quaternion.identity, transform);
                    break;

                // ����
                case 1:
                    // �������̃G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var leftEffect = Instantiate(_slashEffectList[index], position, Quaternion.identity, transform);
                    break;

                // ����
                case 2:
                    // �΂߉E�����̃G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var diagonalRightEffect = Instantiate(_slashEffectList[index], position, Quaternion.identity, transform);

                    // �΂ߍ������̃G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var diagonalLeftEffect = Instantiate(_slashEffectList[index], position, Quaternion.identity, transform);

                    break;

                // ��O�I�ȏ���
                default:
                    Debug.LogError($"Unexpected handIndex value : {handIndex}");
                    break;
            }
        }

        // �s����index�ł���ꍇ
        else
        {
            Debug.LogError($"Effect type not found or Index is out of range.");
        }
    }

    /// <summary>�K�E�Z�G�t�F�N�g�𐶐��E�\������</summary>
    /// <param name="type">�G�t�F�N�g�̑���</param>
    /// <param name="pos">�G�t�F�N�g�̐����ʒu</param>
    /// <param name="player">�v���C���[�̈ʒu</param>
    public void PlayUltEffect(SlashEnchantment type, Vector3 pos, Transform player)
    {
        if (!_ultEffectIndexMap.TryGetValue((type), out int index))
        {
            // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��蓖�Ă��Ă��邽�߁j
            index = -1;
        }

        // index�̒l������ł���ꍇ�A�G�t�F�N�g�𐶐����ăv���C���[�̕����ɍ��킹��
        if (index >= 0 && index < _ultEffectList.Count)
        {
            var effect = Instantiate(_ultEffectList[index], pos, Quaternion.identity, transform);
        }

        // �s����index�ł���ꍇ
        else
        {
            Debug.LogWarning($"Effect type not found or Index is out of range.");
        }
    }

    /// <summary>����U���G�t�F�N�g�i����j�𐶐��E�\������</summary>
    /// <param name="type">�G�t�F�N�g�̑���</param>
    /// <param name="pos">�G�t�F�N�g�̐����ʒu</param>
    /// <param name="player">�v���C���[�̈ʒu</param>
    /// <param name="handIndex">�U���ɗp�����������C���f�b�N�X�i0���E��A1������j</param>
    public void PlayUpperAltEffect(SlashEnchantment type, Vector3 pos, Transform player, int handIndex)
    {
        // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��Ɋ��蓖�Ă��Ă��邽�߁j
        if (!_altEffectIndexMap.TryGetValue((type), out int index)) index = -1;

        // index�̒l������ł���ꍇ
        if (index >= 0 && index < _altEffectList.Count)
        {
            switch (handIndex)
            {
                // �E��
                case 0:
                    // �΂߉E�����̓���U���G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var diagonalRightEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);

                    // �G�t�F�N�g�̃p�[�e�B�N�����Đ����A�K�؂ȃ^�C�~���O�Œ�~������
                    var rightEffectParticle = diagonalRightEffect.GetComponent<ParticleSystem>();

                    break;

                // ����
                case 1:
                    // �΂ߍ������̓���U���G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var diagonalLeftEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);

                    // �G�t�F�N�g�̃p�[�e�B�N�����Đ����A�K�؂ȃ^�C�~���O�Œ�~������
                    var leftEffectParticle = diagonalLeftEffect.GetComponent<ParticleSystem>();

                    break;

                default:
                    Debug.LogError($"Unexpected handIndex value : {handIndex}");
                    break;
            }
        }
    }

    /// <summary>����U���G�t�F�N�g�i�����j�𐶐��E�\������</summary>
    /// <param name="type">�G�t�F�N�g�̑���</param>
    /// <param name="pos">�G�t�F�N�g�̐����ʒu</param>
    /// <param name="player">�v���C���[�̈ʒu</param>
    /// <param name="handIndex">�U���ɗp�����������C���f�b�N�X�i0���E��A1������j</param>
    public void PlayLowerAltEffect(SlashEnchantment type, Vector3 pos, Transform player, int handIndex)
    {
        // �}�b�v�ɑ��݂��Ȃ��ꍇ�́A�C���f�b�N�X��-1�ɐݒ肷��i0�͊��Ɋ��蓖�Ă��Ă��邽�߁j
        if (!_altEffectIndexMap.TryGetValue((type), out int index)) index = -1;

        // index�̒l������ł���ꍇ
        if (index >= 0 && index < _altEffectList.Count)
        {
            switch (handIndex)
            {
                // �E��
                case 0:
                    // �΂߉E�����̓���U���G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var diagonalRightEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);

                    break;

                // ����
                case 1:
                    // �΂ߍ������̓���U���G�t�F�N�g�𐶐����A�����������ɉ�]������
                    var diagonalLeftEffect = Instantiate(_altEffectList[index], pos, Quaternion.identity, transform);

                    break;

                default:
                    Debug.LogError($"Unexpected handIndex value : {handIndex}");
                    break;
            }
        }
    }
}
