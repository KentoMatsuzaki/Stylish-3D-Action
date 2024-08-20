using UnityEngine;
using System.Collections;

/// <summary>����G�t�F�N�g</summary>
public class AltEffect : AttackEffectBase
{
    /// <summary>�p�[�e�B�N�����~������܂ł̒x������</summary>
    [SerializeField] private float _delayDuration = 0.25f;

    /// <summary>Z�������̉�]�p�x(�E)</summary>
    [SerializeField] private float _rotationZAxisRight = 135f;

    /// <summary>Z�������̉�]�p�x(��)</summary>
    [SerializeField] private float _rotationZAxisLeft = 45f;

    /// <summary>�a��グ��G�t�F�N�g���ǂ���</summary>
    [SerializeField] private bool _isLowerEffect = false;

    /// <summary>�E�����̃G�t�F�N�g���ǂ���</summary>
    [SerializeField] private bool _isRightEffect = false;

    private ParticleSystem _particle;

    /// <summary>����G�t�F�N�g�ŗL�̏���</summary>
    protected override void UniqueAction()
    {
        // �G�t�F�N�g����]������
        RotateZAxis();

        // �p�[�e�B�N���̍Đ����~������
        _particle = GetComponent<ParticleSystem>();
        StartCoroutine(StopParticleWithDelay());

        // �v���n�u�̉�]���I���Ă���ړ������邽�߂ɁA���̃^�C�~���O�ōU���N���X���A�N�e�B�u�ɂ���
        GetComponent<AltEffectAttacker>().enabled = true;
    }

    /// <summary>�G�t�F�N�g�̃v���n�u��Z�������ɉ�]������</summary>
    private void RotateZAxis()
    {
        // �E�����E�a��グ
        if (_isLowerEffect && _isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, -_rotationZAxisRight);
        }
        // �������E�a��グ
        else if (_isLowerEffect && !_isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, -_rotationZAxisLeft);
        }
        // �E�����E�a�艺��
        else if (!_isLowerEffect && _isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, _rotationZAxisRight);
        }
        // �������E�a��グ
        else if (!_isLowerEffect && !_isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, _rotationZAxisLeft);
        }
    }

    /// <summary>��莞�ԑҋ@������ɁA�p�[�e�B�N���̍Đ����~������</summary>
    IEnumerator StopParticleWithDelay()
    {
        yield return new WaitForSeconds(_delayDuration);
        _particle.Pause();
    }
}
