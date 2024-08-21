using UnityEngine;

/// <summary>���{�b�g�̏���A�N�V�����Ɋւ���ݒ荀��</summary>

[CreateAssetMenu(fileName = "PatrolSettings", menuName = "ScriptableObjects/PatrolSettings")]

public class PatrolSettings : ScriptableObject
{
    /// <summary>���񂷂�ۂ̈ړ����x</summary>
    [Header("���񎞂̈ړ����x")] public float _patrolSpeed = 1f;

    /// <summary>���񂷂�ۂ̉�]�ɗv���鎞��</summary>
    [Header("���񎞂̉�]����")] public float _patrolRotationDuration = 1.5f;

    /// <summary>���񂷂�ۂ͈̔�</summary>
    [Header("����͈�")] public float _patrolRange = 5f;

    /// <summary>����ڕW�ɓ��B�������𔻒肷��臒l</summary>
    [Header("����ڕW�ւ̓��B臒l")] public float _patrolArrivalThreshold = 0.5f;

    /// <summary>����ڕW��ݒ肷��ۂ̍ŏ���]�p�x</summary>
    [Header("����ڕW�ւ̍ŏ���]�p�x")] public float _minRotationAngle = 45f;

    /// <summary>���񂷂�ۂɃ��C�L���X�g���΂�����</summary>
    [Header("���񎞂̃��C�L���X�g�̋���")] public float _raycastDistance = 1.5f;
}
