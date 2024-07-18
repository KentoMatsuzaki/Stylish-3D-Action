using UnityEngine;

public class PlayerAttacker : MonoBehaviour, IAttacker
{
    /// <summary>プレイヤーの攻撃力</summary>
    [SerializeField, Header("攻撃力")] private int _power;

    /// <summary>攻撃力のプロパティ</summary>
    public int Power
    {
        get => _power; 
        set => _power = value > 0 ? value : 0;
    }

    /// <summary>刀身のコライダー</summary>
    private BoxCollider _collider;

    void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    /// <summary>コライダーを有効化する</summary>
    public void EnableCollider()
    {
        _collider.enabled = true;
    }

    /// <summary>コライダーを無効化する</summary>
    public void DisableCollider()
    {
        _collider.enabled = false;
    }

    /// <summary>攻撃が敵にヒットした際の処理</summary>
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit");
        }
    }
}
