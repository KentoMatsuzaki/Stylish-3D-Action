using UnityEngine;

/// <summary>���{�b�g�̒ǐՃA�N�V�����Ɋւ���ݒ荀��</summary>

[CreateAssetMenu(fileName = "ChaseSettings", menuName = "ScriptableObjects/ChaseSettings")]

public class ChaseSettings : ScriptableObject
{
    /// <summary>�ǐՂ���ۂ̈ړ����x</summary>
    [Header("�ǐՎ��̈ړ����x")] public float _chaseSpeed = 1f;

    /// <summary>�ǐՂ���ۂ̉�]�⊮�̃X�s�[�h�W��</summary>
    [Header("�ǐՎ��̉�]�⊮�W��")] public float _chaseRotationDuration = 3f;

    /// <summary>�ǐՑΏۂ����m���鋗��</summary>
    [Header("�ǐՑΏۂ����m���鋗��")] public float _playerDetectionRange = 5f;

    /// <summary>�ǐՑΏۂɓ��B�������𔻒肷��臒l</summary>
    [Header("�ǐՑΏۂւ̓��B臒l")] public float _chaseArrivalThreshold = 2.5f;
}
