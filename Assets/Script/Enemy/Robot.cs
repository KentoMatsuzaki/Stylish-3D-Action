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

    /// <summary>���񂷂�ڕW�n�_�̌����։�]���ł��邱�Ƃ������t���O</summary>
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
        if (_patrolDestination.HasValue)
        {
            if (IsArrivedAtDestination())
            {
                _patrolDestination = null;
                return NodeStatus.Success;
            }
            else
            {
                if (!_isRotating)
                {
                    MoveForward();
                }   
            }
        }
        else
        {
            SetRandomDestination();
            StartCoroutine(RotateTowardsDestination());
        }
        return NodeStatus.Running;
    }

    //-------------------------------------------------------------------------------
    // ����Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    /// <summary>����̖ڕW�n�_�ɓ��B���Ă��邩�ǂ���</summary>
    private bool IsArrivedAtDestination()
    {
        return Vector3.Distance(transform.position, _patrolDestination.Value) 
            < ARRIVAL_THRESHOLD ? true : false;
    }

    /// <summary>�����_���ȏ���̖ڕW�n�_��ݒ肷��</summary>
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

    IEnumerator RotateTowardsDestination()
    {
        if (_patrolDestination.HasValue)
        {
            var dir = (_patrolDestination.Value - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            _isRotating = true;

            Tween rotationTween = transform.DORotate(lookRotation.eulerAngles, 1.5f);
            yield return rotationTween.WaitForCompletion();

            _isRotating = false;
        }
        else
        {
            Debug.LogError("Destination not Set.");
            yield break;
        }
    }
}
