using System;

/// <summary>�����m�[�h</summary>
public class ConditionNode : BaseNode
{
    /// <summary>�����̕]�����ʂ�Ԃ��f���Q�[�g</summary>
    private readonly Func<bool> _condition;

    /// <summary>�R���X�g���N�^</summary>
    public ConditionNode(Func<bool> condition) => _condition = condition;

    /// <summary>�m�[�h�̕]�������s���郁�\�b�h</summary>
    /// <returns>�m�[�h�̕]�����ʂ�Ԃ�</returns>
    public override NodeStatus Execute()
        => _condition() ? NodeStatus.Success : NodeStatus.Failure;
}
