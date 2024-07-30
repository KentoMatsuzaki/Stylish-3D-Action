using UnityEngine;
using DG.Tweening;
using System.Collections;

/// <summary>�󗤗��p���{�b�g</summary>
public class Robot : MonoBehaviour
{
    /// <summary>�A�j���[�^�[</summary>
    Animator _animator;

    /// <summary>�L�����N�^�[�R���g���[���[</summary>
    CharacterController _controller;

    /// <summary>���񂷂�ڕW�n�_</summary>
    Vector3? _patrolDestination;

    /// <summary>���񂷂�ڕW�n�_�����߂�ۂ̋��̔��a</summary>
    [SerializeField, Header("���񂷂�͈͂̔��a")]  private float _patrolRadius = 5f;

    /// <summary>���񂷂�ۂ̈ړ����x</summary>
    [SerializeField, Header("���񎞂̈ړ����x")] private float _patrolSpeed = 1f;

    /// <summary>���񂷂�ڕW�n�_�ɓ��B�������ǂ����𔻒肷��臒l</summary>
    private const float ARRIVAL_THRESHOLD = 0.5f;

    /// <summary></summary>
    private const float ROTATION_DURATION = 1.5f;

    /// <summary>���񂷂�ڕW�n�_�̕����։�]���ł��邱�Ƃ������t���O</summary>
    private bool _isRotating = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Patrol();
    }

    //-------------------------------------------------------------------------------
    // ����
    //-------------------------------------------------------------------------------

    /// <summary>�ڕW�n�_�����񂳂���</summary>
    /// <returns>����A�N�V�����m�[�h�̕]������</returns>
    public NodeStatus Patrol()
    {
        // �ڕW�n�_�����݂���ꍇ
        if (_patrolDestination.HasValue)
        {
            // �ڕW�n�_�ɓ��B�����ꍇ
            if (IsArrivedAtPatrolDestination())
            {
                // �ڕW�n�_���N���A���āA�����̕]�����ʂ�Ԃ�
                _patrolDestination = null;
                return NodeStatus.Success;
            }
            // �ڕW�n�_�ɓ��B���Ă��Ȃ��ꍇ
            else
            {
                // �ڕW�n�_�̕����։�]���ł���ꍇ
                if (!_isRotating)
                {
                    // �O���ֈړ�������
                    MoveForward();
                }   
            }
        }
        // �ڕW�n�_�����݂��Ȃ��ꍇ
        else
        {
            // �����_���ȖڕW�n�_��ݒ肷��
            SetRandomDestination();

            // �ڕW�n�_�̕����։�]������
            StartCoroutine(RotateTowardsDestination());
        }

        // ���s���̕]�����ʂ�Ԃ�
        return NodeStatus.Running;
    }

    //-------------------------------------------------------------------------------
    // ����Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>���񂷂�ڕW�n�_�ɓ��B���Ă��邩�ǂ���</summary>
    private bool IsArrivedAtPatrolDestination()
    {
        return Vector3.Distance(transform.position, _patrolDestination.Value) 
            < ARRIVAL_THRESHOLD ? true : false;
    }

    /// <summary>���񂷂�ڕW�n�_�������_���ɐݒ肷��</summary>
    private void SetRandomDestination()
    {
        // ����͈͂𔼌a�Ƃ��鋅���́A�����_���Ȓn�_���擾����
        Vector3 randomPos = Random.insideUnitSphere * _patrolRadius;

        // �����_���Ȓn�_��Y���W��0�ɐݒ肷��
        randomPos.y = 0;

        // �����_���Ȓn�_�͌��_�𒆐S�Ƃ��Ă��邽�߁A�L�����N�^�[�̈ʒu�����Z����
        randomPos += transform.position;

        // �ړI�n�������_���Ȓn�_�ɐݒ肷��
        _patrolDestination = randomPos;
    }

    /// <summary>�O���Ɉړ�������</summary>
    private void MoveForward()
    {
        _controller.Move(transform.forward * _patrolSpeed * Time.deltaTime);
    }

    /// <summary>�ڕW�n�_�̕����։�]������R���[�`��</summary>
    IEnumerator RotateTowardsDestination()
    {
        // �ڕW�n�_�����݂���ꍇ
        if (_patrolDestination.HasValue)
        {
            // �ڕW�n�_�ւ̃x�N�g�������߂�
            var dir = (_patrolDestination.Value - transform.position).normalized;

            // �x�N�g������ɁA�ڕW�n�_�ւ̉�]���������߂�
            Quaternion lookRotation = Quaternion.LookRotation(dir);

            // �ڕW�n�_�։�]���ł��邱�Ƃ������t���O���I���ɂ���
            _isRotating = true;

            // �ڕW�n�_�̕����։�]������
            Tween rotationTween = transform.DORotate(lookRotation.eulerAngles, 1.5f);

            // ��]�̊�����ҋ@����
            yield return rotationTween.WaitForCompletion();

            // �ڕW�n�_�։�]���ł��邱�Ƃ������t���O���I�t�ɂ���
            _isRotating = false;
        }
        // �ڕW�n�_�����݂��Ȃ��ꍇ
        else
        {
            // �G���[���O���o�͂��ď����𔲂���
            Debug.LogError("Destination not Set.");
            yield break;
        }
    }
}
