using System;

/// <summary>�A�N�V�����m�[�h</summary>
public class ActionNode : BaseNode
{
    /// <summary>�m�[�h�̕]�����ʂ�Ԃ��f���Q�[�g</summary>
    private readonly Func<NodeStatus> _action;

    /// <summary>�R���X�g���N�^</summary>
    public ActionNode(Func<NodeStatus> action) => _action = action;

    /// <summary>�m�[�h�̕]�������s���郁�\�b�h</summary>
    /// <returns>�m�[�h�̕]�����ʂ�Ԃ�</returns>
    public override NodeStatus Execute() => _action();
}
