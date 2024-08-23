using UnityEngine;

/// <summary>敵の攻撃エフェクトにアタッチする攻撃クラス</summary>
public class EnemyAttacker : MonoBehaviour
{
    /// <summary>攻撃力</summary>
    private int _power;

    public int Power
    {
        get => _power; 
        set => _power = value;
    }

    /// <summary>攻撃エフェクトのコライダー</summary>
    private SphereCollider _collider;

    /// <summary>プレイヤータグの定数</summary>
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
