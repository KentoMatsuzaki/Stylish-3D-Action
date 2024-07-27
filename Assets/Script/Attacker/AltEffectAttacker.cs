using DG.Tweening;
using UnityEngine;

public class AltEffectAttacker : MonoBehaviour, IAttacker
{
    /// <summary>�v���C���[�̍U����</summary>
    [SerializeField, Header("�U����")] private int _power;

    /// <summary>�G�̃Q�[���I�u�W�F�N�g�ɕt�����^�O</summary>
    private const string enemyTag = "Enemy";
    
    /// <summary>����U���G�t�F�N�g���ړ�������Tween</summary>
    private Tween _moveTween;
    
    /// <summary>����U���G�t�F�N�g�̈ړ����x</summary>
    private const float MOVE_SPEED = -10f;

    /// <summary>����U���G�t�F�N�g�̈ړ�����</summary>
    private const float MOVE_DURATION = 1f;

    /// <summary>�U���͂̃v���p�e�B</summary>
    public int Power
    {
        get => _power;
        set => _power = value > 0 ? value : 0;
    }

    /// <summary>����U���G�t�F�N�g�̃R���C�_�[</summary>
    private BoxCollider _collider;

    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        MoveForwardIndefinitely();
    }

    /// <summary>�R���C�_�[��L��������</summary>
    public void EnableCollider()
    {
        _collider.enabled = true;
    }

    /// <summary>�R���C�_�[�𖳌�������</summary>
    public void DisableCollider()
    {
        _collider.enabled = false;
    }

    /// <summary>���̃R���C�_�[�ɐڐG�����ۂɌĂ΂�鏈��</summary>
    public void OnTriggerEnter(Collider other)
    {
        // �G�ɐڐG�����ꍇ
        if (other.CompareTag(enemyTag))
        {
            Debug.Log("Hit");

            // Tween���~������
            _moveTween.Kill();

            // �G�t�F�N�g�̃Q�[���I�u�W�F�N�g��j������
            Destroy(gameObject);
        }
    }

    /// <summary>�G�t�F�N�g�𖳐����ɑO���ֈړ�������</summary>
    private void MoveForwardIndefinitely()
    {
        // �G�t�F�N�g�����݂���ꍇ�́A�������ɑO���ֈړ�������
        _moveTween = transform.DOBlendableMoveBy(transform.forward * MOVE_SPEED, MOVE_DURATION)
            .SetEase(Ease.Linear)
            .OnComplete(() => MoveForwardIndefinitely());
    }
}