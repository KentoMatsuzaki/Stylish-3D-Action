using UnityEngine;
using UnityEngine.InputSystem;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using UnityEngine.UI;
using System;


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
    private PlayerState _currentState = PlayerState.Idle;

    /// <summary>���s���̈ړ����x</summary>
    [SerializeField, Header("���s���̈ړ����x")] private float _defaultSpeed = 4.0f;

    /// <summary>���s���̈ړ����x</summary>
    [SerializeField, Header("���s���̈ړ����x")] private float _sprintSpeed = 7.5f;

    /// <summary>�E��̍U���N���X</summary>
    [SerializeField, Header("�E��̍U���N���X")] private PlayerAttacker _rightSwordAttacker;

    /// <summary>����̍U���N���X</summary>
    [SerializeField, Header("����̍U���N���X")] private PlayerAttacker _leftSwordAttacker;

    /// <summary>�U������̎�������</summary>
    [SerializeField, Header("�U������̎�������")] private float _attackDuration;



    [SerializeField] Text _text;

    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
        _groundCheck = GetComponent<GroundCheck>();
        _jumpControl = GetComponent<JumpControl>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // �ړ��t���O��ݒ�
        _animator.SetBool("IsMove", _moveControl.IsMove);

        // ���[�g���[�V�����̓K�p�t���O��ݒ�
        _animator.applyRootMotion = _currentState == PlayerState.Attack ? true : false;

        // �e�X�g�p
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
    // �v���C���[�̏�ԂɊւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�v���C���[�̏�Ԃ𖳑����Ԃɐݒ肷��</summary>
    /// <summary>�A�j���[�V�����C�x���g����Ă΂��</summary>
    public void SetPlayerStateIdle ()
    {
        _currentState = PlayerState.Idle;
    }

    /// <summary>�v���C���[�̏�Ԃ��ړ���Ԃɐݒ肷��</summary>
    /// <summary>�A�j���[�V�����C�x���g����Ă΂��</summary>
    public void SetPlayerStateMove()
    {
        _currentState = PlayerState.Move;
    }

    //-------------------------------------------------------------------------------
    // �ړ��̃R�[���o�b�N�C�x���g
    //-------------------------------------------------------------------------------

    /// <summary>�ړ��̐��������R�[���o�b�N�C�x���g</summary>
    /// <summary>PlayerInput�R���|�[�l���g����Ă΂��</summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        // �ړ����󂯕t���Ȃ��ꍇ�͏����𔲂���
        if(!CanMove()) return;

        // ���͒l��臒l�iPress�j�ȏ�ɂȂ����ꍇ
        if (context.performed)
        {
            // ���͒l�����Ɍv�Z�����ړ������ւƈړ�������
            _moveControl.Move(context.ReadValue<Vector2>());

            // �ړ���ԂɑJ�ڂł���ꍇ
            if (CanTransitionToMoveState())
            {
                // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
                _animator.Play("Move");
                _currentState = PlayerState.Move;
            }
        }

        // ���͒l��臒l�iRelease�j�ȉ��ɂȂ����ꍇ
        else if (context.canceled)
        {
            // �ړ�������0�ɂ���i�ړ������Ȃ��悤�ɂ���j
            _moveControl.Move(Vector2.zero);

            // �ړ���Ԃ̏ꍇ
            if (_currentState == PlayerState.Move)
            {
                // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
                _animator.Play("Move End");
                _currentState = PlayerState.Idle;
            }

            // �X�v�����g��Ԃ̏ꍇ
            else if(_currentState == PlayerState.Sprint)
            {
                // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
                _animator.Play("Sprint End");
                _currentState = _moveControl.IsMove ? PlayerState.Move : PlayerState.Idle;
            }
        }
    }

    //-------------------------------------------------------------------------------
    // �ړ��Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�ړ���ԂɑJ�ڂł��邩�ǂ���</summary>
    private bool CanTransitionToMoveState()
    {
        if (_currentState == PlayerState.Idle) return true;
        return false;
    }

    /// <summary>�ړ����󂯕t���邩�ǂ���</summary>
    private bool CanMove()
    {
        if (_currentState == PlayerState.Attack || _currentState == PlayerState.Damage ||
            _currentState == PlayerState.Die) return false;
        else return true;
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

            // �t���O��L��������
            _animator.SetBool("IsSprint", true);

            // �X�v�����g��ԂɑJ�ڂł���ꍇ
            if(CanTransitionToSprintState())
            {
                // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
                _animator.Play("Sprint");
                _currentState = PlayerState.Sprint;
            }
        }

        // ���͒l��臒l�iRelease�j�ȉ��ɂȂ����ꍇ
        else if (context.canceled)
        {
            // ���s���̈ړ����x�ɕύX����
            _moveControl.MoveSpeed = _defaultSpeed;

            // �t���O�𖳌�������
            _animator.SetBool("IsSprint", false);

            // �X�v�����g��Ԃ̏ꍇ
            if(_currentState == PlayerState.Sprint)
            {
                // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
                _animator.Play("Sprint End");
                _currentState = _moveControl.IsMove ? PlayerState.Move : PlayerState.Idle;
            } 
        }
    }

    //-------------------------------------------------------------------------------
    // �X�v�����g�Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�X�v�����g��ԂɑJ�ڂł��邩�ǂ���</summary>
    private bool CanTransitionToSprintState()
    {
        if (_currentState == PlayerState.Move) return true;
        return false;
    }

    //-------------------------------------------------------------------------------
    // �W�����v�̃R�[���o�b�N�C�x���g
    //-------------------------------------------------------------------------------

    /// <summary>�W�����v�̐��������R�[���o�b�N�C�x���g</summary>
    /// <summary>PlayerInput�R���|�[�l���g����Ă΂��</summary>
    public void OnJump(InputAction.CallbackContext context)
    {
        // �W�����v��ԂɑJ�ڂł���ꍇ
        if (!CanTransitionToJumpState()) return;

        // ���͒l��臒l�iPress�j�ȏ�ɂȂ����ꍇ
        if (context.performed)
        {
            // �W�����v������
            _jumpControl.Jump(true);

            // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
            _animator.Play("Jump Start");
            _currentState = PlayerState.Jump;
        }
    }

    /// <summary>���n���̏���������R�[���o�b�N�C�x���g</summary>
    /// <summary>Gravity�R���|�[�l���g����Ă΂��</summary>
    public void OnLand()
    {
        // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
        _animator.Play("Jump End");
        _currentState = !_moveControl.IsMove ? PlayerState.Idle :
        _moveControl.MoveSpeed == _defaultSpeed ? PlayerState.Move : PlayerState.Sprint;
    }

    //-------------------------------------------------------------------------------
    // �W�����v�Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�W�����v��ԂɑJ�ڂł��邩�ǂ���</summary>
    private bool CanTransitionToJumpState()
    {
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Move ||
            _currentState == PlayerState.Sprint) return true;
        return false;
    }

    //-------------------------------------------------------------------------------
    // �U���̃R�[���o�b�N�C�x���g
    //-------------------------------------------------------------------------------

    /// <summary>�U���̐��������R�[���o�b�N�C�x���g</summary>
    /// <summary>PlayerInput�R���|�[�l���g����Ă΂��</summary>
    public void OnAttack(InputAction.CallbackContext context)
    {
        // �U���A�N�V���������������ꂽ�ꍇ
        if (context.performed)
        {
            // �U����ԂɑJ�ڂł���ꍇ
            if (CanTransitionToAttackState())
            {
                // �g���K�[��L�������āA�v���C���[�̏�Ԃ��X�V����
                _animator.SetTrigger("Attack");
                _currentState = PlayerState.Attack;
            }  
        }
    }

    //-------------------------------------------------------------------------------
    // �U���Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�U����ԂɑJ�ڂł��邩�ǂ���</summary>
    private bool CanTransitionToAttackState()
    {
        if (_currentState == PlayerState.Idle || _currentState == PlayerState.Attack) return true;
        return false;
    }

    /// <summary>�U�����q�b�g����u�Ԃ̏���������R�[���o�b�N�C�x���g</summary>
    /// <summary>�A�j���[�V�����C�x���g����Ă΂��</summary>
    /// <param name="handIndex">�U���ɗp�����������C���f�b�N�X�i0���E��A1������A2������j</param>
    public void AttackImpactEvent(int handIndex)
    {
        switch(handIndex)
        {
            case 0:
                TriggerRightSwordCollider();
                break;

            case 1:
                TriggerLeftSwordCollider();
                break;

            case 2:
                TriggerRightSwordCollider();
                TriggerLeftSwordCollider();
                break;

            default:
                Debug.LogError($"Unexpected handIndex value : {handIndex}");
                break;
        }
    }

    /// <summary>�E��Ŏ����Ă��镐��̃R���C�_�[���������Ԃ����L��������</summary>
    private void TriggerRightSwordCollider()
    {
        EnableRightSwordCollider();
        Invoke(nameof(DisableRightSwordCollider), _attackDuration);
    }

    /// <summary>����Ŏ����Ă��镐��̃R���C�_�[���������Ԃ����L��������</summary>
    private void TriggerLeftSwordCollider()
    {
        EnableLeftSwordCollider();
        Invoke(nameof(DisableLeftSwordCollider), _attackDuration);
    }

    public void EnableRightSwordCollider() => _rightSwordAttacker.EnableCollider();

    public void DisableRightSwordCollider() => _rightSwordAttacker.DisableCollider();

    public void EnableLeftSwordCollider() => _leftSwordAttacker.EnableCollider();

    public void DisableLeftSwordCollider() => _leftSwordAttacker.DisableCollider();

    /// <summary>�a���G�t�F�N�g�i�E�j�𐶐��E�\������</summary>
    public void PlayRightSlashEffect(string effectType)
    {
        // �����Ŏw�肵��������enum�ɃL���X�g�ł���ꍇ�A�Ή�����G�t�F�N�g��\������
        if(Enum.TryParse<AttackEffectType>(effectType, out var result))
        {
            Vector3 playerPos = transform.position;
            var effectPos = new Vector3(playerPos.x, playerPos.y + 1.25f, playerPos.z);
            EffectManager.Instance.PlayRightSlashEffect(result, effectPos, transform);
        }
        else
        {
            Debug.LogWarning("The given string is incorrect.");
        }
    }
}
