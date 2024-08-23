using UnityEngine;

/// <summary>�G�̍U���G�t�F�N�g�ɃA�^�b�`����U���N���X</summary>
public class EnemyAttacker : MonoBehaviour
{
    /// <summary>�U����</summary>
    private int _power;

    public int Power
    {
        get => _power; 
        set => _power = value;
    }

    /// <summary>�U���G�t�F�N�g�̃R���C�_�[</summary>
    private SphereCollider _collider;

    /// <summary>�v���C���[�^�O�̒萔</summary>
    private const string PLAYER_TAG = "Player";

    private void Start()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            Debug.Log("Damage Player");
            Destroy(gameObject);
        }
    }
}
