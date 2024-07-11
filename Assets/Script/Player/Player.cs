using System;
using System.Collections.Generic;
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

    /// <summary>�v���C���[�̏��</summary>
    private State _currentState;

    /// <summary>��Ԃ̃R���N�V����</summary>
    private Dictionary<Type, State> _stateDic;

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

        _stateDic = new Dictionary<Type, State>
        {
            { typeof(IdleState), new IdleState(this) },
            { typeof(TrotState), new TrotState(this) },
            { typeof(SprintState), new SprintState(this) },
            { typeof(JumpState), new JumpState(this) },
            { typeof(AttackState), new AttackState(this) },
            { typeof(DamageState), new DamageState(this) },
            { typeof(DeadState), new DeadState(this) },
        };

        ChangeState<IdleState>();
    }

    /// <summary>�v���C���[�̏�Ԃ�J�ڂ�����</summary>
    private void ChangeState<T>() where T : State
    {
        // ���݂̏�ԂƑJ�ڐ�̏�Ԃ������ꍇ�͑������^�[��
        if (_currentState is T) return;

        // ��Ԃ�J�ڂ�����
        _currentState?.Exit();
        _currentState = _stateDic[typeof(T)];
        _currentState.Enter();
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

            // ��Ԃ�J��
            if(!(_currentState is SprintState)) ChangeState<TrotState>();
        }

        // ���͒l��臒l�iRelease�j�ȉ��ɂȂ����ꍇ
        else if (context.canceled)
        { 
            // �ړ����Ȃ��悤�ɂ���
            _moveControl.Move(Vector2.zero);

            // ��Ԃ�J��
            ChangeState<IdleState>();
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

            // ��Ԃ�J��
            ChangeState<SprintState>();
        }

        // ���͒l��臒l�iRelease�j�ȉ��ɂȂ����ꍇ
        else if (context.canceled)
        {
            // ���s���̈ړ����x�ɕύX����
            _moveControl.MoveSpeed = _walkSpeed;

            // ��Ԃ�J��
            if (_moveControl.IsMove) ChangeState<TrotState>();
            else ChangeState<IdleState>();
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
