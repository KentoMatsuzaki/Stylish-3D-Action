using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Robot))]

/// <summary>ロボットのエディタ拡張</summary>
public class EnemyEditor : Editor
{
    /// <summary>シーンビューにギズモを描画する</summary>
    void OnSceneGUI()
    {
        // 選択中のロボットのインスタンスを取得する
        Robot robot = (Robot)target;

        // ギズモの色を設定して、感知範囲の円を描画
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(robot.transform.position, Vector3.up, robot.DetectionRange);
    }
}
