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
    /// <param name="hand">�U���Ɏg�p������\���񋓌^</param>
    public void CreateSlashEffect(Vector3 position, AttackHand hand)
    {
        switch(hand)
        {
            // �E��
            case AttackHand.Right:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._rightSlashEffect,
                    position, Quaternion.identity, transform); break;

            // ����
            case AttackHand.Left:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._leftSlashEffect,
                    position, Quaternion.identity, transform); break;

            // ����̏ꍇ�́A�E�ƍ��̃G�t�F�N�g�𗼕���������
            case AttackHand.Both:
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._rightSlashEffect,
                    position, Quaternion.identity, transform);
                Instantiate(_attackEffectList[(int)Player.Instance.Enchantment]._leftSlashEffect,
                    position, Quaternion.identity, transform); break;

            // ��O�I�ȏ���
            default:
                Debug.LogError($"Invalid hand selected : {hand}"); break;
        }
    }

    
}
