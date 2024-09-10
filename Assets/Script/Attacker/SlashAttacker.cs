using UnityEngine;

/// <summary>斬撃攻撃クラス</summary>
public class SlashAttacker : MonoBehaviour, IAttacker
{
    /// <summary>プレイヤーの攻撃力</summary>
    [SerializeField, Header("攻撃力")] private int _power;

    /// <summary>攻撃力のプロパティ</summary>
    public int Power
    {
        get => _power; 
        set => _power = value > 0 ? value : 0;
    }

    /// <summary>のコライダー</summary>
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
}
