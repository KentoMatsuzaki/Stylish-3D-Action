using UnityEngine;

/// <summary>�G�̍U���G�t�F�N�g�ɃA�^�b�`����N���X</summary>
public class EnemyAttackEffect : MonoBehaviour
{
    /// <summary>�G�t�F�N�g�̎��</summary>
    public enum EnemyAttackEffectType
    {
        /// <summary>���i</summary>
        Straight,

        /// <summary>�ǔ�</summary>
        Homing
    }

    /// <summary>�U���G�t�F�N�g�̎��</summary>
    [SerializeField, Header("�U���G�t�F�N�g�̎��")] private EnemyAttackEffectType _effectType;

    /// <summary>�U���G�t�F�N�g�̈ړ����x</summary>
    [SerializeField, Header("�U���G�t�F�N�g�̈ړ����x")] private float _moveSpeed;

    /// <summary>�ǔ����̉�]�⊮�W��</summary>
    [SerializeField, Header("�ǔ����̉�]�⊮�W��")] private float _rotationSlerpSpeed;

    private void Update()
    {
        switch(_effectType)
        {
            // ���i����G�t�F�N�g
            case EnemyAttackEffectType.Straight:
                MoveForward(); break;

            // �ǔ�����G�t�F�N�g
            case EnemyAttackEffectType.Homing:
                HomingPlayer(); break;
        }
    }

    /// <summary>�O���Ɉړ�����</summary>
    private void MoveForward()
    {
        transform.Translate(transform.forward * _moveSpeed * Time.deltaTime);
    }

    /// <summary>�v���C���[��ǔ�����</summary>
    private void HomingPlayer()
    {
        // �v���C���[�̕����։�]����
        RotateTowardsPlayer();

        // �O���Ɉړ�����
        MoveForward();
    }

    /// <summary>�v���C���[�ւ�Y���W�𖳎��������������߂�</summary>
    private Vector3 GetHorizontalDirectionToPlayer()
    {
        Vector3 dir = (Player.Instance.transform.position - transform.position).normalized;
        Vector3 fixedDir = new Vector3(dir.x, 0, dir.z);
        return fixedDir;
    }

    /// <summary>�v���C���[�ւ�Y���W�𖳎�������]�����߂�</summary>
    private Quaternion GetHorizontalRotationToPlayer()
    {
        return Quaternion.LookRotation(GetHorizontalDirectionToPlayer());
    }

    /// <summary>�v���C���[�̕����։�]����</summary>
    private void RotateTowardsPlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, GetHorizontalRotationToPlayer(),
            _rotationSlerpSpeed * Time.deltaTime);
    }
}
