using System;
using System.Collections.Generic;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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

    /// <summary>���݂̃v���C���[�̏��</summary>
    private PlayerState _currentState;

    /// <summary>���s���̈ړ����x</summary>
    [SerializeField, Header("���s���̈ړ����x")] private float _walkSpeed = 1.2f;

    /// <summary>���s���̈ړ����x</summary>
    [SerializeField, Header("���s���̈ړ����x")] private float _sprintSpeed = 4.0f;



    [SerializeField] Text _text;

    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
        _groundCheck = GetComponent<GroundCheck>();
        _jumpControl = GetComponent<JumpControl>();
        _animator = GetComponent<Animator>();
        // �v���C���[�̏�Ԃ�������
        _currentState = PlayerState.Idle;
    }

    private void Update()
    {
        _text.text = _currentState.ToString();
    }

    //-------------------------------------------------------------------------------
    // �v���C���[�̏��
    //-------------------------------------------------------------------------------

    private enum PlayerState
    {
        Idle, �@// ������
        Move, �@// �ړ�
        Sprint, // �X�v�����g
        Jump, �@// �W�����v
        Attack, // �U��
        Damage, // �_���[�W
        Die �@�@// ���S
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

            // �ړ���ԂɑJ�ڂł���ꍇ
            if (CanTransitionToMoveState())
            {
                // �A�j���[�V�������Đ�
                _animator.Play("Move");

                // �v���C���[�̏�Ԃ��X�V
                _currentState = PlayerState.Move;
            }
        }

        // ���͒l��臒l�iRelease�j�ȉ��ɂȂ����ꍇ
        else if (context.canceled)
        {
            // �ړ����Ȃ��悤�ɂ���
            _moveControl.Move(Vector2.zero);

            // �ړ���Ԃ̏ꍇ
            if (_currentState == PlayerState.Move)
            {
                // �A�j���[�V�������Đ�
                _animator.Play("Move End");

                // �v���C���[�̏�Ԃ��X�V
                _currentState = PlayerState.Idle;
            }
        }
    }

    //-------------------------------------------------------------------------------
    // �ړ��Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�ړ���ԂɑJ�ڂł��邩�ǂ���</summary>
    private bool CanTransitionToMoveState()
    {
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Move) return true;
        return false;
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

            // �X�v�����g��ԂɑJ�ڂł���ꍇ
            if(CanTransitionToSprintState())
            {
                // �A�j���[�V�������Đ�
                _animator.Play("Sprint");

                // �v���C���[�̏�Ԃ��X�V
                _currentState = PlayerState.Sprint;
            }
        }

        // ���͒l��臒l�iRelease�j�ȉ��ɂȂ����ꍇ
        else if (context.canceled)
        {
            // ���s���̈ړ����x�ɕύX����
            _moveControl.MoveSpeed = _walkSpeed;

            // �X�v�����g��Ԃ̏ꍇ
            if(_currentState == PlayerState.Sprint)
            {
                // �A�j���[�V�������Đ�
                if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Sprint")) _animator.Play("Sprint End");

                // �v���C���[�̏�Ԃ��X�V
                _currentState = PlayerState.Idle;
            } 
        }
    }

    //-------------------------------------------------------------------------------
    // �X�v�����g�Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�X�v�����g��ԂɑJ�ڂł��邩�ǂ���</summary>
    private bool CanTransitionToSprintState()
    {
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Move ||
            _currentState == PlayerState.Sprint) return true;
        return false;
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

            // �W�����v��Ԃł͂Ȃ��ꍇ
            if (_currentState != PlayerState.Jump)
            {
                // �A�j���[�V�������Đ�
                _animator.Play("Jump Start");

                // �v���C���[�̏�Ԃ��X�V
                _currentState = PlayerState.Jump;
            }
        }
    }

    /// <summary>���n���̏���������R�[���o�b�N�C�x���g</summary>
    /// <summary>Gravity�R���|�[�l���g����Ă΂��</summary>
    public void OnLand()
    {
        // �A�j���[�V�������Đ�
        _animator.Play("Jump End");

        // �v���C���[�̏�Ԃ��X�V
        _currentState = PlayerState.Idle;
    }
}
