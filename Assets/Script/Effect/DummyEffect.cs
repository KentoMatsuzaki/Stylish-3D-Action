using UnityEngine;

/// <summary>攻撃時に付加効果がない場合に生成するダミーエフェクト</summary>
public class DummyEffect : MonoBehaviour
{
    void Start()
    {
        // ダミーエフェクトを破棄する
        Destroy(gameObject);
    }
}
