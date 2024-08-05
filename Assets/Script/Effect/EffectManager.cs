using UnityEngine;
using System.Collections.Generic;

/// <summary>�e��G�t�F�N�g���Ǘ�����N���X</summary>
public class EffectManager : Singleton<EffectManager>
{
    /// <summary>�U���G�t�F�N�g�𐶐�����ۂ̍����̃I�t�Z�b�g</summary>
    [SerializeField] private float _attackEffectHeightOffset = 1.25f;

    /// <summary>�U���G�t�F�N�g�f�[�^�̃��X�g</summary>
    [SerializeField, Header("�U���G�t�F�N�g�f�[�^�̃��X�g")]
        private List<AttackEffectData> _attackEffectList = new List<AttackEffectData>();

    //-------------------------------------------------------------------------------
    // �U���G�t�F�N�g�𐶐����鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�a���G�t�F�N�g�𐶐����ĕ\�����郁�\�b�h</summary>
    /// <param name="hand">�U���Ɏg�p������\���񋓌^</param>
    public void CreateSlashEffect(AttackHand hand)
    {
        switch(hand)
        {
            // �E��
            case AttackHand.Right:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._rightSlashEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // ����
            case AttackHand.Left:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._leftSlashEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // ����̏ꍇ�́A�E�ƍ��̃G�t�F�N�g�𗼕���������
            case AttackHand.Both:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._rightSlashEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform);
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._leftSlashEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // ��O�I�ȏ���
            default:
                Debug.LogError($"Invalid hand selected : {hand}"); break;
        }
    }

    /// <summary>����G�t�F�N�g�i�a��グ�j�𐶐����ĕ\�����郁�\�b�h</summary>
    /// <param name="hand">�U���Ɏg�p������\���񋓌^</param>
    public void CreateLowerAltEffect(AttackHand hand)
    {
        switch (hand)
        {
            // �E��
            case AttackHand.Right:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._lowerRightAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // ����
            case AttackHand.Left:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._lowerLeftAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // ����̏ꍇ�́A�E�ƍ��̃G�t�F�N�g�𗼕���������
            case AttackHand.Both:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._lowerRightAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform);
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._lowerLeftAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // ��O�I�ȏ���
            default:
                Debug.LogError($"Invalid hand selected : {hand}"); break;
        }
    }

    /// <summary>����G�t�F�N�g�i�a�艺���j�𐶐����ĕ\�����郁�\�b�h</summary>
    /// <param name="hand">�U���Ɏg�p������\���񋓌^</param>
    public void CreateUpperAltEffect(AttackHand hand)
    {
        switch (hand)
        {
            // �E��
            case AttackHand.Right:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._upperRightAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // ����
            case AttackHand.Left:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._upperLeftAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // ����̏ꍇ�́A�E�ƍ��̃G�t�F�N�g�𗼕���������
            case AttackHand.Both:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._upperRightAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform);
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._upperLeftAltEffect,
                    GetAttackEffectPosition(), Quaternion.identity, transform); break;

            // ��O�I�ȏ���
            default:
                Debug.LogError($"Invalid hand selected : {hand}"); break;
        }
    }

    //-------------------------------------------------------------------------------
    // �U���G�t�F�N�g�̐����Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�U���G�t�F�N�g�𐶐�����ʒu�����߂�</summary>
    private Vector3 GetAttackEffectPosition()
    {
        // �v���C���[�̈ʒu�ɃI�t�Z�b�g���������ʒu�����߂ĕԂ�
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 effectPos = new Vector3(playerPos.x, playerPos.y + _attackEffectHeightOffset, playerPos.z);
        return effectPos;
    }
}