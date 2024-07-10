using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>�v���C���[�̏�ԊǗ�</summary>
public class PlayerStateMachine : MonoBehaviour
{
    /// <summary>�ړ�����</summary>
    private MoveControl _moveControl;

    /// <summary>�W�����v����</summary>
    private JumpControl _jumpControl;

    /// <summary>�ڒn����</summary>
    private GroundCheck _groundCheck;

    /// <summary>�A�j���[�^�[</summary>
    private Animator _animator;

    /// <summary>�v���C���[�̏��</summary>
    private PlayerState _currentState = PlayerState.Idle;

    /// <summary>���s���̈ړ����x</summary>
    [SerializeField, Header("���s���̈ړ����x")] private float _walkSpeed = 1.2f;

    /// <summary>���s���̈ړ����x</summary>
    [SerializeField, Header("���s���̈ړ����x")] private float _sprintSpeed = 4.0f;

    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
        _groundCheck = GetComponent<GroundCheck>();
        _jumpControl = GetComponent<JumpControl>();
        _animator = GetComponent<Animator>();
    }

    //-------------------------------------------------------------------------------
    // �X�e�[�g�}�V��
    //-------------------------------------------------------------------------------

    private enum PlayerState
    {
        Idle, // ��������
        Walking, // ���s��
        Running, // ���s��
        Jumping, // �W�����v��
        Attack, // �U�����
        Damaged, // ��_���[�W���
        Dead // ���S���
    }

    /// <summary>�������Ԃ̏���</summary>
    private void HandleIdleState()
    {
        Debug.Log("�v���C���[�̏�ԁFIdle");
    }

    /// <summary>���s���̏���</summary>
    private void HandleWalkingState()
    {
        Debug.Log("�v���C���[�̏�ԁFWalking");
    }

    /// <summary>���s���̏���</summary>
    private void HandleRunningState()
    {
        Debug.Log("�v���C���[�̏�ԁFRunning");
    }

    /// <summary>�W�����v���̏���</summary>
    private void HandleJumpingState()
    {
        Debug.Log("�v���C���[�̏�ԁFJumping");
    }

    /// <summary>�U����Ԃ̏���</summary>
    private void HandleAttackState()
    {
        Debug.Log("�v���C���[�̏�ԁFAttack");
    }

    /// <summary>��_���[�W��Ԃ̏���</summary>
    private void HandleDamagedState()
    {
        Debug.Log("�v���C���[�̏�ԁFDamaged");
    }

    /// <summary>���S��Ԃ̏���</summary>
    private void HandleDeadState()
    {
        Debug.Log("�v���C���[�̏�ԁFDead");
    }

    /// <summary>�v���C���[�̏�Ԃ�J�ڂ�����</summary>
    private void TransitionToOtherState(PlayerState otherState)
    {
        // ���݂̏�ԂƑJ�ڐ�̏�Ԃ������ꍇ�A�����𔲂���
        if (_currentState == otherState) return;

        // ���݂̏�Ԃ�J�ڂ�����
        _currentState = otherState;

        // �C�x���g���Ă�
        OnStateTransition();
    }

    /// <summary>�e��Ԃ̌ŗL���������s����C�x���g�B��Ԃ̑J�ڎ��ɌĂ΂��B</summary>
    private void OnStateTransition()
    {
        switch(_currentState)
        {
            case PlayerState.Idle:
                HandleIdleState(); 
                break;

            case PlayerState.Walking:
                HandleWalkingState();
                break;

            case PlayerState.Running:
                HandleRunningState();
                break;

            case PlayerState.Jumping:
                HandleJumpingState();
                break;

            case PlayerState.Attack:
                HandleAttackState();
                break;

            case PlayerState.Damaged:
                HandleDamagedState();
                break;

            case PlayerState.Dead:
                HandleDeadState();
                break;
        }
    }

    //-------------------------------------------------------------------------------
    // �ړ��̃R�[���o�b�N�C�x���g
    //-------------------------------------------------------------------------------

    /// <summary>�ړ��̐��������R�[���o�b�N�C�x���g</summary>
    /// <summary>PlayerInput�R���|�[�l���g����Ă΂��</summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // ���s��ԂɑJ�ڂ���
        TransitionToOtherState(PlayerState.Walking);

        // ���͒l��臒l�iPress�j�ȏ�ɂȂ����ꍇ
        if (context.performed)
        {
            // ���͒l�����Ɍv�Z�����ړ������ւƈړ�������
            _moveControl.Move(context.ReadValue<Vector2>());
        }

        // ���͒l��臒l�iRelease�j�ȉ��ɂȂ����ꍇ
        else if (context.canceled)
        { 
            // �ړ����Ȃ��悤�ɂ���
            _moveControl.Move(Vector2.zero);
        }
    }

    //-------------------------------------------------------------------------------
    // �X�v�����g�̃R�[���o�b�N�C�x���g
    //-------------------------------------------------------------------------------

    /// <summary>�X�v�����g�̐��������R�[���o�b�N�C�x���g</summary>
    /// <summary>PlayerInput�R���|�[�l���g����Ă΂��</summary>
    public void OnSprint(InputAction.CallbackContext context)
    {
        // ���s��ԂɑJ�ڂ���
        TransitionToOtherState(PlayerState.Running);

        // ���͒l��臒l�iPress�j�ȏ�ɂȂ����ꍇ
        if (context.performed)
        {
            // ���s���̈ړ����x�ɕύX����
            _moveControl.MoveSpeed = _sprintSpeed;
        }

        // ���͒l��臒l�iRelease�j�ȉ��ɂȂ����ꍇ
        else if (context.canceled)
        {
            // ���s���̈ړ����x�ɕύX����
            _moveControl.MoveSpeed = _walkSpeed;
        }
    }

    //-------------------------------------------------------------------------------
    // �W�����v�̃R�[���o�b�N�C�x���g
    //-------------------------------------------------------------------------------

    /// <summary>�W�����v�̐��������R�[���o�b�N�C�x���g</summary>
    /// <summary>PlayerInput�R���|�[�l���g����Ă΂��</summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // �W�����v��ԂɑJ�ڂ���
        TransitionToOtherState(PlayerState.Jumping);

        // ���͒l��臒l�iPress�j�ȏ�ɂȂ����ꍇ
        if (context.performed)
        {
            // �W�����v������
            _jumpControl.Jump(true);
        }
    }
}
