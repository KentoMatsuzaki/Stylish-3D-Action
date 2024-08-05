using UnityEngine;
using System.Collections;

/// <summary>特殊エフェクト</summary>
public class AltEffect : AttackEffectBase
{
    /// <summary>パーティクルを停止させるまでの遅延時間</summary>
    [SerializeField] private float _delayDuration = 0.25f;

    private ParticleSystem _particle;

    protected override void Start()
    {
        // 基底クラスのStartメソッドを呼び出す
        base.Start();

        // 固有の処理を呼び出す
        UniqueAction();
    }

    /// <summary>特殊エフェクト固有の処理</summary>
    protected override void UniqueAction()
    {
        _particle = GetComponent<ParticleSystem>();
        StartCoroutine(StopParticleWithDelay());
    }

    /// <summary>一定時間待機した後に、パーティクルの再生を停止させる</summary>
    IEnumerator StopParticleWithDelay()
    {
        yield return new WaitForSeconds(_delayDuration);
        _particle.Pause();
    }
}
