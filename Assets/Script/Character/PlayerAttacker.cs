using UnityEngine;

public class PlayerAttacker : MonoBehaviour, IAttacker
{
    /// <summary>�v���C���[�̍U����</summary>
    [SerializeField, Header("�U����")] private int _power;

    /// <summary>�G�̃Q�[���I�u�W�F�N�g�ɕt�����^�O</summary>
    private const string enemyTag = "Enemy";

    /// <summary>�U���͂̃v���p�e�B</summary>
    public int Power
    {
        get => _power; 
        set => _power = value > 0 ? value : 0;
    }

    /// <summary>���g�̃R���C�_�[</summary>
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

    /// <summary>�U�����G�Ƀq�b�g�����ۂ̏���</summary>
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == enemyTag)
        {
            Debug.Log("Hit");
        }
    }
}
