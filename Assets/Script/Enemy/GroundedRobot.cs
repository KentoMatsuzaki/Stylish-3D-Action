using UnityEngine;

public class GroundedRobot : MonoBehaviour
{
    Animator _animator;

    enum RobotState
    {
        Patrol, // ����
        Chase, // �ǐ�
        Attack, // �U��
        Damage, // ��_���[�W
        Dead // ���S
    }
}
