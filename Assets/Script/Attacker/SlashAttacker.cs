using UnityEngine;

/// <summary>�a���U���N���X</summary>
public class SlashAttacker : MonoBehaviour, IAttacker
{
    /// <summary>�v���C���[�̍U����</summary>
    [SerializeField, Header("�U����")] private int _power;

    /// <summary>�U���͂̃v���p�e�B</summary>
    public int Power
    {
        get => _power; 
        set => _power = value > 0 ? value : 0;
    }

    /// <summary>�̃R���C�_�[</summary>
    private BoxCollider _collider;

    void Start()
    {
        _collider = GetComponent<BoxCollider>();
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
}
