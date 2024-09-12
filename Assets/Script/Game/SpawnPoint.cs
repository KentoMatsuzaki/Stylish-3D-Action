using UnityEngine;

/// <summary>敵のスポーン地点にアタッチするクラス</summary>
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject _spawnEffect;

    public void EnableSpawnEffect()
    {
        _spawnEffect.SetActive(true);
    }

    public void DisableSpawnEffect()
    {
        _spawnEffect.SetActive(false);
    }

    public void EnableSpawnEffectTemporarily()
    {
        EnableSpawnEffect();
        Invoke(nameof(DisableSpawnEffect), 2.0f);
    }
}
