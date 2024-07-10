using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;

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
    
    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
        _groundCheck = GetComponent<GroundCheck>();
        _jumpControl = GetComponent<JumpControl>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (_currentState)
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
        }
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

    }

    /// <summary>���s���̏���</summary>
    private void HandleWalkingState()
    {

    }

    /// <summary>���s���̏���</summary>
    private void HandleRunningState()
    {

    }

    /// <summary>�W�����v���̏���</summary>
    private void HandleJumpingState()
    {

    }

    /// <summary>�U����Ԃ̏���</summary>
    private void HandleAttackState()
    {

    }

    /// <summary>��_���[�W��Ԃ̏���</summary>
    private void HandleDamagedState()
    {

    }

    /// <summary>���S��Ԃ̏���</summary>
    private void HandleDeadState()
    {

    }

    /// <summary>��Ԃ̑J��</summary>
    private void TransitionState(PlayerState newState)
    {
        _currentState = newState;
    }

    //-------------------------------------------------------------------------------
    // �ړ��̃R�[���o�b�N�C�x���g
    //-------------------------------------------------------------------------------

    /// <summary>�ړ��̕������Z���s��</summary>
    /// <summary>PlayerInput����Ă΂��</summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // ���͒l��臒l�iPress�j�ȏ�ɂȂ����ꍇ
        if (context.performed)
        {
            // �ړ��������v�Z
            _moveControl.Move(context.ReadValue<Vector2>());
        }

        // ���͒l��臒l�iRelease�j�ȉ��ɂȂ����ꍇ
        else if (context.canceled)
        { 
            // �ړ������𖳌���
            _moveControl.Move(Vector2.zero);
        }

        TransitionState(PlayerState.Walking);
    }
}
