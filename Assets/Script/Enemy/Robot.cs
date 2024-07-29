using DG.Tweening;
using System.Collections;
using UnityEngine;

/// <summary>�󗤗��p���{�b�g</summary>
public class Robot : MonoBehaviour
{
    /// <summary>�A�j���[�^�[</summary>
    Animator _animator;

    /// <summary>���{�b�g�̌��݂̏��</summary>
    RobotState _currentState = RobotState.Initialize;

    CharacterController _controller;

    Vector3? _destination;
    float _patrolRange = 2.5f;
    float _moveSpeed = 1f;
    float _arrivalThreshold = 0.5f;

    float timer = 0f;
    bool _isRotating = false;

    /// <summary>���{�b�g�̏�Ԃ�\���񋓌^</summary>
    private enum RobotState
    {
        /// <summary>�������̏��</summary>
        Initialize,

        /// <summary>������</summary>
        Patrol,

        /// <summary>�ǐՏ��</summary>
        Chase,

        /// <summary>�U�����</summary>
        Attack,

        /// <summary>��_���[�W���</summary>
        Damage,

        /// <summary>���S���</summary>
        Dead
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if(_currentState == RobotState.Patrol)
        {
            Patrol();
        }

        timer += Time.deltaTime;

        if (timer > 1f && _destination.HasValue)
        {
            Debug.Log(_destination.Value.ToString());
            Debug.Log(Vector3.Distance(transform.position, _destination.Value));
            timer = 0f;
        }
    }

    //-------------------------------------------------------------------------------
    // ����
    //-------------------------------------------------------------------------------

    public NodeStatus Patrol()
    {
        if (_destination.HasValue)
        {
            if (IsArrivedAtDestination())
            {
                _destination = null;
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
    
    public void SetRobotStatePatrol()
    {
        _currentState = RobotState.Patrol;
    }

    /// <summary>����̖ڕW�n�_�ɓ��B���Ă��邩�ǂ���</summary>
    private bool IsArrivedAtDestination()
    {
        return Vector3.Distance(transform.position, _destination.Value) 
            < _arrivalThreshold ? true : false;
    }

    /// <summary>�����_���ȏ���̖ڕW�n�_��ݒ肷��</summary>
    private void SetRandomDestination()
    {
        // ����͈͂𔼌a�Ƃ��鋅���́A�����_���Ȓn�_���擾����
        Vector3 randomPos = Random.insideUnitSphere * _patrolRange;

        // �����_���Ȓn�_��Y���W��0�ɐݒ肷��
        randomPos.y = 0;

        // �����_���Ȓn�_�͌��_�𒆐S�Ƃ��Ă��邽�߁A�L�����N�^�[�̈ʒu�����Z����
        randomPos += transform.position;

        // �ړI�n�������_���Ȓn�_�ɐݒ肷��
        _destination = randomPos;
    }

    /// <summary>�O���Ɉړ�������</summary>
    private void MoveForward()
    {
        _controller.Move(transform.forward * _moveSpeed * Time.deltaTime);
    }

    IEnumerator RotateTowardsDestination()
    {
        if (_destination.HasValue)
        {
            var dir = (transform.position - _destination.Value).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            _isRotating = true;

            Tween rotationTween = transform.DORotate(lookRotation.eulerAngles, 5f);
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
