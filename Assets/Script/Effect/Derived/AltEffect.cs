using UnityEngine;
using System.Collections;

/// <summary>特殊エフェクト</summary>
public class AltEffect : AttackEffectBase
{
    /// <summary>パーティクルを停止させるまでの遅延時間</summary>
    [SerializeField] private float _delayDuration = 0.25f;

    /// <summary>Z軸方向の回転角度(右)</summary>
    [SerializeField] private float _rotationZAxisRight = 135f;

    /// <summary>Z軸方向の回転角度(左)</summary>
    [SerializeField] private float _rotationZAxisLeft = 45f;

    /// <summary>斬り上げるエフェクトかどうか</summary>
    [SerializeField] private bool _isLowerEffect = false;

    /// <summary>右方向のエフェクトかどうか</summary>
    [SerializeField] private bool _isRightEffect = false;

    private ParticleSystem _particle;

    /// <summary>特殊エフェクト固有の処理</summary>
    protected override void UniqueAction()
    {
        // エフェクトを回転させる
        RotateZAxis();

        // パーティクルの再生を停止させる
        _particle = GetComponent<ParticleSystem>();
        StartCoroutine(StopParticleWithDelay());

        // プレハブの回転を終えてから移動させるために、このタイミングで攻撃クラスをアクティブにする
        GetComponent<AltEffectAttacker>().enabled = true;
    }

    /// <summary>エフェクトのプレハブをZ軸方向に回転させる</summary>
    private void RotateZAxis()
    {
        // 右方向・斬り上げ
        if (_isLowerEffect && _isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, -_rotationZAxisRight);
        }
        // 左方向・斬り上げ
        else if (_isLowerEffect && !_isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, -_rotationZAxisLeft);
        }
        // 右方向・斬り下げ
        else if (!_isLowerEffect && _isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, _rotationZAxisRight);
        }
        // 左方向・斬り上げ
        else if (!_isLowerEffect && !_isRightEffect)
        {
            transform.rotation *= Quaternion.Euler(0, 0, _rotationZAxisLeft);
        }
    }

    /// <summary>一定時間待機した後に、パーティクルの再生を停止させる</summary>
    IEnumerator StopParticleWithDelay()
    {
        yield return new WaitForSeconds(_delayDuration);
        _particle.Pause();
    }
}
