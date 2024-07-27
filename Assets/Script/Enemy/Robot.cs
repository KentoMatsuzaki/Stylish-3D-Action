using UnityEngine;

/// <summary>�󗤗��p���{�b�g</summary>
public class Robot : MonoBehaviour
{
    /// <summary>�A�j���[�^�[</summary>
    Animator _animator;

    /// <summary>�R���g���[���[</summary>
    CharacterController _controller;

    /// <summary>���{�b�g�̌��݂̏��</summary>
    RobotState _currentState;

    /// <summary>���{�b�g�̏�Ԃ�\���񋓌^</summary>
    private enum RobotState
    {
        /// <summary>������</summary>
        Patrol,

        /// <summary>�ǐՏ��</summary>
        Chase,

        /// <summary>�U�����</summary>
        Attack,

        /// <summary>��_���[�W���</summary>
        Damage,

        /// <summary>���S���</summary>
        Dead
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    //-------------------------------------------------------------------------------
    // ����
    //-------------------------------------------------------------------------------

    //public NodeStatus Patrol()
    //{
        
    //}

    //-------------------------------------------------------------------------------
    // ����Ɋւ��鏈��
    //-------------------------------------------------------------------------------
}
