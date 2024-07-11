using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>�v���C���[�̏�ԊǗ�</summary>
public class Player : MonoBehaviour
{
    /// <summary>�ړ�����</summary>
    private MoveControl _moveControl;

    /// <summary>�W�����v����</summary>
    private JumpControl _jumpControl;

    /// <summary>�ڒn����</summary>
    private GroundCheck _groundCheck;

    /// <summary>�A�j���[�^�[</summary>
    private Animator _animator;

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
    // �ړ��̃R�[���o�b�N�C�x���g
    //-------------------------------------------------------------------------------

    /// <summary>�ړ��̐��������R�[���o�b�N�C�x���g</summary>
    /// <summary>PlayerInput�R���|�[�l���g����Ă΂��</summary>
    public void OnMove(InputAction.CallbackContext context)
    {
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
        // ���͒l��臒l�iPress�j�ȏ�ɂȂ����ꍇ
        if (context.performed)
        {
            // �W�����v������
            _jumpControl.Jump(true);
        }
    }
}
