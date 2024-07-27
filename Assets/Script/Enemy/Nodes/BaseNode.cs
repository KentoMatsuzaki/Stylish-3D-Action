/// <summary>BehaviourTree���\������m�[�h�̃x�[�X�N���X</summary>
public abstract class BaseNode
{
    /// <summary>�m�[�h�̕]�������s���郁�\�b�h</summary>
    /// <returns>�m�[�h�̕]�����ʂ�Ԃ�</returns>
    public abstract NodeStatus Execute();
}

/// <summary>�m�[�h�̕]�����ʂ�\���񋓌^</summary>
public enum NodeStatus
{
    /// <summary>�m�[�h�̕]���������������Ƃ�����</summary>
    Success,

    /// <summary>�m�[�h�̕]�������s�������Ƃ�����</summary>
    Failure,

    /// <summary>�m�[�h�̕]�������s���ł��邱�Ƃ�����</summary>
    Running
}
