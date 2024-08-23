using UnityEngine;

/// <summary>ロボットの追跡アクションに関する設定項目</summary>

[CreateAssetMenu(fileName = "ChaseSettings", menuName = "ScriptableObjects/ChaseSettings")]

public class ChaseSettings : ScriptableObject
{
    /// <summary>追跡する際の移動速度</summary>
    [Header("追跡時の移動速度")] public float _chaseSpeed = 1f;

    /// <summary>追跡する際の回転補完のスピード係数</summary>
    [Header("追跡時の回転補完係数")] public float _chaseRotationSlerpSpeed = 3f;

    /// <summary>追跡対象を感知する距離</summary>
    [Header("追跡対象を感知する距離")] public float _playerDetectionRange = 5f;

    /// <summary>追跡対象に到達したかを判定する閾値</summary>
    [Header("追跡対象への到達閾値")] public float _chaseArrivalThreshold = 2.5f;
}
