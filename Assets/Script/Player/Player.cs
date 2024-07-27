using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Brain;
using Unity.TinyCharacterController.Effect;
using Unity.TinyCharacterController.Control;

/// <summary>�v���C���[�̏�ԊǗ�</summary>
public class Player : MonoBehaviour
{
    /// <summary>�ړ�����</summary>
    private MoveControl _moveControl;

    /// <summary>�W�����v����</summary>
    private JumpControl _jumpControl;

    /// <summary>�ڒn����</summary>
    private GroundCheck _groundCheck;

    /// <summary>�d�͐���</summary>
    private Gravity _gravity;

    /// <summary>���W�b�N����</summary>
    private CharacterBrain _brain;

    /// <summary>�A�j���[�^�[</summary>
    private Animator _animator;

    /// <summary>���݂̃v���C���[�̏��</summary>
    private PlayerState _currentState = PlayerState.Idle;

    /// <summary>���s���̈ړ����x</summary>
    [SerializeField, Header("���s���̈ړ����x")] private float _defaultSpeed = 4.0f;

    /// <summary>���s���̈ړ����x</summary>
    [SerializeField, Header("���s���̈ړ����x")] private float _sprintSpeed = 7.5f;

    /// <summary>�E��̍U���N���X</summary>
    [SerializeField, Header("�E��̍U���N���X")] private SwordAttacker _rightSwordAttacker;

    /// <summary>����̍U���N���X</summary>
    [SerializeField, Header("����̍U���N���X")] private SwordAttacker _leftSwordAttacker;

    /// <summary>�U������̎�������</summary>
    [SerializeField, Header("�U������̎�������")] private float _attackDuration;

    /// <summary>�U���̑���</summary>
    [SerializeField, Header("�U���̑���")] public AttackEffectType _attackEffectType;

    /// <summary>���V�ɏ�����G�l���M�[</summary>
    [SerializeField, Header("���V�G�l���M�[")] private float _floatEnergy = 1f;

    /// <summary>�G��T������͈͂̔��a</summary>
    [SerializeField, Header("�G��T������͈͂̔��a")] private float _detectionRadius = 5f;

    /// <summary>�G�̃��C���[�}�X�N</summary>
    [SerializeField, Header("�G�̃��C���[�}�X�N")] private LayerMask _enemyLayer;

    [SerializeField] Text _text;

    void Start()
    {
        _moveControl = GetComponent<MoveControl>();
        _groundCheck = GetComponent<GroundCheck>();
        _jumpControl = GetComponent<JumpControl>();
        _gravity = GetComponent<Gravity>();
        _brain = GetComponent<CharacterBrain>();
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
        Float, // ���V
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

        // �ړ��A�N�V�����Ɋ��蓖�Ă�ꂽ�L�[�o�C���h�����͂��ꂽ�ꍇ
        if (context.performed)
        {
            // ���͒l�̈ړ������ֈړ�������
            _moveControl.Move(context.ReadValue<Vector2>());

            // �ړ���ԂɑJ�ڂł���ꍇ
            if (CanTransitionToMoveState())
            {
                // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
                _animator.Play("Move");
                _currentState = PlayerState.Move;
            }
        }

        // �ړ��A�N�V�����Ɋ��蓖�Ă�ꂽ�L�[�o�C���h�������ꂽ�ꍇ
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
        if (_currentState == PlayerState.Attack || _currentState == PlayerState.Float || 
            _currentState == PlayerState.Damage || _currentState == PlayerState.Die) return false;
        else return true;
    }

    //-------------------------------------------------------------------------------
    // �X�v�����g�̃R�[���o�b�N�C�x���g
    //-------------------------------------------------------------------------------

    /// <summary>�X�v�����g�̐��������R�[���o�b�N�C�x���g</summary>
    /// <summary>PlayerInput�R���|�[�l���g����Ă΂��</summary>
    public void OnSprint(InputAction.CallbackContext context)
    {
        // �X�v�����g�A�N�V�����Ɋ��蓖�Ă�ꂽ�L�[�o�C���h�����͂��ꂽ�ꍇ
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

        // �X�v�����g�A�N�V�����Ɋ��蓖�Ă�ꂽ�L�[�o�C���h�������ꂽ�ꍇ
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

        // �W�����v�A�N�V�����Ɋ��蓖�Ă�ꂽ�L�[�o�C���h�����͂��ꂽ�ꍇ
        if (context.performed)
        {
            // �W�����v������
            _jumpControl.Jump(true);

            // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
            _animator.Play("Jump");
            _currentState = PlayerState.Jump;
        }
    }

    /// <summary>���n���̏���������R�[���o�b�N�C�x���g</summary>
    /// <summary>Gravity�R���|�[�l���g����Ă΂��</summary>
    public void OnLand()
    {
        // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
        _animator.Play("Land");
        _currentState = !_moveControl.IsMove ? PlayerState.Idle :
            _moveControl.MoveSpeed == _defaultSpeed ? PlayerState.Move : PlayerState.Sprint;

        _jumpControl.JumpHeight = 2f;
        _gravity.GravityScale = 1.5f;
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
        // �U���A�N�V�����Ɋ��蓖�Ă�ꂽ�L�[�o�C���h�����͂��ꂽ�ꍇ
        if (context.performed)
        {
            // �U����ԂɑJ�ڂł���ꍇ
            if (CanTransitionToAttackState())
            {
                // �U���g���K�[��L�������āA�v���C���[�̏�Ԃ��X�V����
                _animator.SetTrigger("Attack");
                _currentState = PlayerState.Attack;

                LookAtClosestEnemy(true);
            }  

            // ���V�i����U���j��Ԃ̏ꍇ
            else if(_currentState == PlayerState.Float)
            {
                // ����U���g���K�[��L��������
                _animator.SetTrigger("Alt");

                LookAtClosestEnemy(false);
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
            // �E��
            case 0:
                TriggerRightSwordCollider();
                break;

            // ����
            case 1:
                TriggerLeftSwordCollider();
                break;

            // ����
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

    /// <summary>�a���G�t�F�N�g�𐶐��E�\������</summary>
    /// <summary>�A�j���[�V�����C�x���g����Ă΂��</summary>
    /// <param name="handIndex">�U���ɗp�����������C���f�b�N�X�i0���E��A1������A2������j</param>
    public void PlaySlashEffect(int handIndex)
    {
        // �v���C���[�̍��W�����ɁA�������ʒu�ɃG�t�F�N�g�𐶐�����
        Vector3 playerPos = transform.position;
        var effectPos = new Vector3(playerPos.x, playerPos.y + 1.25f, playerPos.z);
        EffectManager.Instance.PlaySlashEffect(_attackEffectType, effectPos, transform, handIndex);
    }

    /// <summary>�a���G�t�F�N�g�i�����j�𐶐��E�\������</summary>
    /// <summary>�A�j���[�V�����C�x���g����Ă΂��</summary>
    /// <param name="handIndex">�U���ɗp�����������C���f�b�N�X�i0���E��A1������j</param>
    public void PlayLowerAltEffect(int handIndex)
    {
        // �v���C���[�̍��W�����ɁA�������ʒu�ɃG�t�F�N�g�𐶐�����
        Vector3 playerPos = transform.position;
        var effectPos = new Vector3(playerPos.x, playerPos.y + 1.25f, playerPos.z);
        EffectManager.Instance.PlayLowerAltEffect(_attackEffectType, effectPos, transform, handIndex);
    }

    /// <summary>�a���G�t�F�N�g�i����j�𐶐��E�\������</summary>
    /// <summary>�A�j���[�V�����C�x���g����Ă΂��</summary>
    /// <param name="handIndex">�U���ɗp�����������C���f�b�N�X�i0���E��A1������j</param>
    public void PlayUpperAltEffect(int handIndex)
    {
        // �v���C���[�̍��W�����ɁA�������ʒu�ɃG�t�F�N�g�𐶐�����
        Vector3 playerPos = transform.position;
        var effectPos = new Vector3(playerPos.x, playerPos.y + 1.25f, playerPos.z);
        EffectManager.Instance.PlayUpperAltEffect(_attackEffectType, effectPos, transform, handIndex);
    }

    /// <summary>�T���͈͓��Ńv���C���[����ł��߂��ʒu�ɂ���G��T������</summary>
    /// <returns>�ł��߂��G�̈ʒu</returns>
    private Transform FindClosestEnemy()
    {
        // �v���C���[�̈ʒu�𒆐S�ɁA�w�肳�ꂽ���a���ɂ���S�Ă̓G���擾����
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, _detectionRadius, _enemyLayer);
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (var enemy in enemyColliders)
        {
            // �v���C���[�ƓG�̈ʒu�̍����v�Z���A�����̕��������߂�
            var distanceSqr = (enemy.transform.position - transform.position).sqrMagnitude;

            // ���݂̍ŒZ�����Ɣ�r���āA���߂��G�����������ꍇ�A�ŒZ�����ƍł��߂��G���X�V����
            if (distanceSqr < closestDistanceSqr)
            {
                closestEnemy = enemy.transform;
                closestDistanceSqr = distanceSqr;
            }
        }

        // �ł��߂��G�����������ꍇ�A���̓G��Transform��Ԃ��B
        if (closestEnemy != null)
        {
            return closestEnemy;
        }
        // ������Ȃ������ꍇ�A���O���o�͂���null��Ԃ��B
        else
        {
            Debug.Log("No enemy found in detection range.");
            return null;
        }
    }

    /// <summary>�ł��߂��G����������Ƀv���C���[����]������</summary>
    private void LookAtClosestEnemy(bool isNormalAttack)
    {
        // �ł��߂��G�̈ʒu���擾����
        Transform closestEnemyPos = FindClosestEnemy();

        // �ł��߂��G��������Ȃ������ꍇ�A���O���o�͂��ď����𔲂���
        if (closestEnemyPos == null)
        {
            Debug.Log("No enemy found to look at.");
            return;
        }

        // �v���C���[�ƓG�̈ʒu�̍����v�Z���A�ڕW�ƂȂ���W�����߂�
        Vector3 targetPosition = closestEnemyPos.position - transform.position;

        if (isNormalAttack)
        {
            // �ʏ�U���̏ꍇ��Y���̉�]�ʂ𖳎�����
            targetPosition.y = 0;

            // �ڕW�ƂȂ���W�܂ł̉�]�ʂ����߂�
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

            // �v���C���[����]������
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 100f);
        }
        else
        { 
            // �ڕW�ƂȂ���W�܂ł̉�]�ʂ����߂�
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition);

            // �v���C���[����]������
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1f);
        }  
    }

    //-------------------------------------------------------------------------------
    // ���V�̃R�[���o�b�N�C�x���g
    //-------------------------------------------------------------------------------

    /// <summary>���V�̐��������R�[���o�b�N�C�x���g</summary>
    /// <summary>PlayerInput�R���|�[�l���g����Ă΂��</summary>
    public void OnFloat(InputAction.CallbackContext context)
    {
        // ���V�A�N�V�����Ɋ��蓖�Ă�ꂽ�L�[�o�C���h�����͂��ꂽ�ꍇ
        if (context.performed)
        {
            // ���V��ԂɑJ�ڂł���ꍇ
            if (CanTransitionToFloatState())
            {
                // ���V������
                _jumpControl.JumpHeight = 4f;
                _jumpControl.Jump();

                // �A�j���[�V�������Đ����āA�v���C���[�̏�Ԃ��X�V����
                _animator.Play("Float Jump");
                _currentState = PlayerState.Float;
            }

            // ���V��Ԃł���ꍇ
            else if(_currentState == PlayerState.Float)
            {
                _brain.SetFreezeAxis(false, false, false);
                _gravity.enabled = true;
                _gravity.GravityScale = 0.75f;
            }
        }
    }

    //-------------------------------------------------------------------------------
    // ���V�Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>���V��ԂɑJ�ڂł��邩�ǂ���</summary>
    private bool CanTransitionToFloatState()
    {
        if (_currentState == PlayerState.Idle && _floatEnergy > 0) return true;
        return false;
    }

    /// <summary>���V�C�x���g</summary>
    /// <summary>�A�j���[�V�����C�x���g����Ă΂��</summary>
    public void Float()
    {
        FreezeYAxis();
        DisableGravity();
    }

    /// <summary>�v���C���[��Y���W���Œ肷��</summary>
    private void FreezeYAxis()
    {
        _brain.SetFreezeAxis(false, true, false);
    }

    /// <summary>�d�͂𖳌�������</summary>
    private void DisableGravity()
    {
        _gravity.enabled = false;
    }
}
