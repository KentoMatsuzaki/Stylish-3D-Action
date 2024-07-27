using UnityEngine;

public class GroundedRobot : MonoBehaviour
{
    Animator _animator;

    enum RobotState
    {
        Patrol, // „‰ñ
        Chase, // ’ÇÕ
        Attack, // UŒ‚
        Damage, // ”íƒ_ƒ[ƒW
        Dead // €–S
    }
}
