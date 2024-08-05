using UnityEngine;
using System.Collections;

public class AltEffect : AttackEffectBase
{
    /// <summary>�p�[�e�B�N�����~������܂ł̒x������</summary>
    [SerializeField] private float _delayDuration = 0.25f;

    private ParticleSystem _particle;

    protected override void Start()
    {
        // ���N���X��Start���\�b�h���Ăяo��
        base.Start();

        // �ŗL�̏������Ăяo��
        UniqueAction();
    }

    /// <summary>����G�t�F�N�g�ŗL�̏���</summary>
    protected override void UniqueAction()
    {
        _particle = GetComponent<ParticleSystem>();
        StartCoroutine(StopParticleWithDelay());
    }

    /// <summary>��莞�ԑҋ@������ɁA�p�[�e�B�N���̍Đ����~������</summary>
    IEnumerator StopParticleWithDelay()
    {
        yield return new WaitForSeconds(_delayDuration);
        _particle.Pause();
    }
}
