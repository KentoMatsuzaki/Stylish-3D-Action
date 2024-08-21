using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Robot))]

/// <summary>���{�b�g�̃G�f�B�^�g��</summary>
public class EnemyEditor : Editor
{
    /// <summary>�V�[���r���[�ɃM�Y����`�悷��</summary>
    void OnSceneGUI()
    {
        // �I�𒆂̃��{�b�g�̃C���X�^���X���擾����
        Robot robot = (Robot)target;

        // �M�Y���̐F��ݒ肵�āA���m�͈͂̉~��`��
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(robot.transform.position, Vector3.up, robot.PlayerDetectionRange);
    }
}
