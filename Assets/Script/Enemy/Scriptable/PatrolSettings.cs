using UnityEngine;

/// <summary>ロボットの巡回アクションに関する設定項目</summary>

[CreateAssetMenu(fileName = "PatrolSettings", menuName = "ScriptableObjects/PatrolSettings")]

public class PatrolSettings : ScriptableObject
{
    /// <summary>巡回する際の移動速度</summary>
    [Header("巡回時の移動速度")] public float _patrolSpeed = 1f;

    /// <summary>巡回する際の回転に要する時間</summary>
    [Header("巡回時の回転時間")] public float _patrolRotationDuration = 1.5f;

    /// <summary>巡回する際の範囲</summary>
    [Header("巡回範囲")] public float _patrolRange = 5f;

    /// <summary>巡回目標に到達したかを判定する閾値</summary>
    [Header("巡回目標への到達閾値")] public float _patrolArrivalThreshold = 0.5f;

    /// <summary>巡回目標を設定する際の最小回転角度</summary>
    [Header("巡回目標への最小回転角度")] public float _minRotationAngle = 45f;

    /// <summary>巡回する際にレイキャストを飛ばす距離</summary>
    [Header("巡回時のレイキャストの距離")] public float _raycastDistance = 1.5f;
}
