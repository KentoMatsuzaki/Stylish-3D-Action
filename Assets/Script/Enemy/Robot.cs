using UnityEngine;

/// <summary>�󗤗��p���{�b�g</summary>
public class Robot : MonoBehaviour
{
    /// <summary>�A�j���[�^�[</summary>
    Animator _animator;

    /// <summary>���{�b�g�̌��݂̏��</summary>
    RobotState _currentState;

    CharacterController _controller;

    Vector3? _destination;
    float _patrolRange = 5f;
    float _moveSpeed = 1f;
    float _arrivalThreshold = 0.5f;

    float timer = 0f;

    /// <summary>���{�b�g�̏�Ԃ�\���񋓌^</summary>
    private enum RobotState
    {
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
        Patrol();
        timer += Time.deltaTime;
        if (timer > 1f)
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
            if (IsArrived())
            {
                _destination = null;
                return NodeStatus.Success;
            }
            else
            {
                Vector3 direction = (_destination.Value - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                _controller.Move(transform.forward * _moveSpeed * Time.deltaTime);
                return NodeStatus.Running;
            }
        }
        else
        {
            SetRandomDestination();
            return NodeStatus.Running;
        }
    }

    //-------------------------------------------------------------------------------
    // ����Ɋւ��鏈��
    //-------------------------------------------------------------------------------

    private bool IsArrived()
    {
        return Vector3.Distance(transform.position, _destination.Value) 
            < _arrivalThreshold ? true : false;
    }

    private void SetRandomDestination()
    {
        // ����͈͂𔼌a�Ƃ��鋅���́A�����_���Ȓn�_���擾����
        Vector3 randomPos = Random.insideUnitSphere * _patrolRange;

        randomPos.y = 0;

        //Quaternion rotation = Quaternion.LookRotation(randomPos);

        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

        

        // ���͌��_�𒆐S�Ƃ��Ă��邽�߁A�G�̈ʒu��������
        randomPos += transform.position;

        // 
        _destination = randomPos;

        

    }
}
