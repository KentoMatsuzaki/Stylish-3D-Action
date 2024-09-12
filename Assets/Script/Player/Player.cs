using Unity.TinyCharacterController.Brain;
using Unity.TinyCharacterController.Check;
using Unity.TinyCharacterController.Control;
using Unity.TinyCharacterController.Effect;
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

    /// <summary>�U���N���X</summary>
    [SerializeField, Header("�U���N���X")] private SlashAttacker _attacker;

    /// <summary>�U������̎�������</summary>
    [SerializeField, Header("�U������̎�������")] private float _attackDuration;

    /// <summary>�a���̑���</summary>
    [SerializeField, Header("�U���̑���")] private SlashEnchantment _enchantment;

    public SlashEnchantment Enchantment => _enchantment;

    /// <summary>���V�ɏ�����G�l���M�[</summary>
    [SerializeField, Header("���V�G�l���M�[")] private float _floatEnergy = 1f;

    /// <summary>�G��T������͈͂̔��a</summary>
    [SerializeField, Header("�G��T������͈͂̔��a")] private float _detectionRadius = 5f;

    /// <summary>�G�̃��C���[�}�X�N</summary>
    [SerializeField, Header("�G�̃��C���[�}�X�N")] private LayerMask _enemyLayer;

    /// <summary>�v���C���[��HP</summary>
    [SerializeField, Header("�v���C���[�̗̑�")] private float _hp = 1000;

    /// <summary>��_���[�W���̖��G�t���O</summary>
    private bool _invincibleFlag = false;

    /// <summary>���G��Ԃ�\���G�t�F�N�g</summary>
    [SerializeField, Header("���G�G�t�F�N�g")] private GameObject _invincibleEffect;

    /// <summary>�v���C���[�̃C���X�^���X</summary>
    public static Player Instance {  get; private set; }

    [SerializeField] Text _text;

    private void Awake()
    {
        // �C���X�^���X�����݂��Ȃ��ꍇ
        if (Instance == null)
        {
            // �C���X�^���X��o�^����
            Instance = this;
        }

        // �C���X�^���X�����݂���ꍇ
        else
        {
            // �Q�[���I�u�W�F�N�g��j������
            Destroy(gameObject);
        }
    }

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
        Dead �@�@// ���S
    }

    //-------------------------------------------------------------------------------
    // �������ԂɊւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>�v���C���[�̏�Ԃ𖳑����Ԃɐݒ肷��</summary>
    /// <summary>�A�j���[�V�����C�x���g����Ă΂��</summary>
    public void SetPlayerStateIdle ()
    {
        _currentState = PlayerState.Idle;
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

    /// <summary>�v���C���[�̏�Ԃ��ړ���Ԃɐݒ肷��</summary>
    /// <summary>�A�j���[�V�����C�x���g����Ă΂��</summary>
    public void SetPlayerStateMove()
    {
        _currentState = PlayerState.Move;
    }

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
            _currentState == PlayerState.Damage || _currentState == PlayerState.Dead) return false;
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
    public void AttackImpactEvent()
    {
        CancelInvoke(nameof(DisableAttackCollider));
        EnableAttackCollider();
        Invoke(nameof(DisableAttackCollider), _attackDuration);
    }

    /// <summary>�U���p�̃R���C�_�[���U���̎������Ԃ����L��������</summary>
    private void EnableAttackCollider()
    {
        _attacker.EnableCollider();
    }

    /// <summary>�U���p�̃R���C�_�[�𖳌�������</summary>
    private void DisableAttackCollider()
    {
        _attacker.DisableCollider();
    }


    /// <summary>�U���G�t�F�N�g(�a��グ)�𐶐��E�\������</summary>
    /// <param name="attackHandIndex">�U���Ɏg�p�����������C���f�b�N�X</param>
    private void PlayLowerSlashEffect(int attackHandIndex)
    {
        EffectManager.Instance.CreateLowerSlashEffect((AttackHand)attackHandIndex);
    }

    /// <summary>�U���G�t�F�N�g(�a�艺��)�𐶐��E�\������</summary>
    /// <param name="attackHandIndex">�U���Ɏg�p�����������C���f�b�N�X</param>
    private void PlayUpperSlashEffect(int attackHandIndex)
    {
        EffectManager.Instance.CreateUpperSlashEffect((AttackHand)attackHandIndex);
    }

    /// <summary>�U���G�t�F�N�g(����)�𐶐��E�\������</summary>
    /// <param name="attackHandIndex">�U���Ɏg�p�����������C���f�b�N�X</param>
    private void PlayHorizontalSlashEffect(int attackHandIndex)
    {
        EffectManager.Instance.CreateHorizontalSlashEffect((AttackHand)attackHandIndex);
    }

    /// <summary>�T���͈͓��Ńv���C���[����ł��߂��G�̈ʒu��Ԃ�</summary>
    /// <returns>�ł��߂��G�̈ʒu</returns>
    private Transform FindClosestEnemyPos()
    {
        // �v���C���[�̈ʒu�𒆐S�ɁA�w�肳�ꂽ���a���ɂ���S�Ă̓G���擾����
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, _detectionRadius, _enemyLayer);
        Transform closestEnemyPos = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (var enemyCollider in enemyColliders)
        {
            // �v���C���[�ƓG�̈ʒu�̍����v�Z���A�����̕��������߂�
            var distanceSqr = (enemyCollider.transform.position - transform.position).sqrMagnitude;

            // ���݂̍ŒZ�����Ɣ�r���āA���߂��G�����������ꍇ�A�ŒZ�����ƍł��߂��G���X�V����
            if (distanceSqr < closestDistanceSqr)
            {
                closestEnemyPos = enemyCollider.transform;
                closestDistanceSqr = distanceSqr;
            }
        }

        // �ł��߂��G�̈ʒu��Ԃ�
        if (closestEnemyPos != null)
        {
            return closestEnemyPos;
        }
        // �T���͈͓��ɓG�����Ȃ������ꍇ�́A���O���o�͂���null��Ԃ�
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
        Transform closestEnemyPos = FindClosestEnemyPos();

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

                // ���݂̉�]���擾
                Quaternion currentRotation = transform.rotation;

                // Y���̉�]���ێ����AX����Z���̉�]��0�ɐݒ�
                Quaternion fixedRotation = Quaternion.Euler(0, currentRotation.eulerAngles.y, 0);

                // �C���ς݂̉�]��K�p
                transform.rotation = fixedRotation;
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
    private void FreezeYAxis() => _brain.SetFreezeAxis(false, true, false);

    /// <summary>�d�͂𖳌�������</summary>
    private void DisableGravity() => _gravity.enabled = false;

    //-------------------------------------------------------------------------------
    // �_���[�W����
    //-------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider other)
    {
        // �G�̍U���R���C�_�[�ƐڐG�����ꍇ
        if (other.gameObject.CompareTag("EnemyAttack") && CanTransitionToDamageState())
        {
            // �_���[�W�K�p
            TakeDamage(other.gameObject.GetComponent<EnemyAttacker>().Power);
        }
    }

    /// <summary>�_���[�W��ԂɑJ�ڂł��邩�ǂ���</summary>
    private bool CanTransitionToDamageState()
    {
        if (_currentState == PlayerState.Float || _currentState == PlayerState.Damage ||
            _currentState == PlayerState.Dead) return false;
        return true;
    }

    /// <summary>�_���[�W�K�p����</summary>
    private void TakeDamage(int damage)
    {
        if (_invincibleFlag) return;

        _hp -= damage;

        // �̗͂�0�ȉ��Ȃ玀�S�������Ă�
        if (_hp < 0)
        {
            OnDead();
        }

        // �̗͂�0�ȏ�Ȃ�_���[�W�������Ă�
        else
        {
            OnDamage();
        }
    }

    /// <summary>��_���[�W���̏���</summary>
    private void OnDamage()
    {
        _currentState = PlayerState.Damage;
        _animator.SetTrigger("Damage");
        EnableInvincibility();
        Invoke(nameof(DisableInvincibility), 2.0f);
    }

    /// <summary>���G��Ԃ̗L����</summary>
    private void EnableInvincibility()
    {
        _invincibleFlag = true;
        _invincibleEffect.SetActive(true);
    }

    /// <summary>���G��Ԃ̖�����</summary>
    private void DisableInvincibility()
    {
        _invincibleFlag = false;
        _invincibleEffect.SetActive(false);
    }

    /// <summary>���S���̏���</summary>
    private void OnDead()
    {
        _currentState = PlayerState.Dead;
        _animator.Play("Die");
    }
}
