using UnityEngine;

/// <summary>�U���G�t�F�N�g�̊��N���X</summary>
public abstract class AttackEffectBase : MonoBehaviour
{
    protected virtual void Start()
    {
        // �v���C���[�̐��ʕ����։�]������
        transform.rotation = Quaternion.LookRotation(-Player.Instance.transform.forward);

        UniqueAction();
    }

    /// <summary>�h���N���X�ŃI�[�o�[���C�h���钊�ۃ��\�b�h</summary>
    protected abstract void UniqueAction();
}