using System.Collections.Generic;

/// <summary>�V�[�P���X�m�[�h</summary>
public class SequenceNode : BaseNode
{
    /// <summary>�q�m�[�h�̃��X�g</summary>
    private readonly List<BaseNode> _children = new List<BaseNode>();

    /// <summary>�q�m�[�h�̃��X�g�ɐV���Ȏq�m�[�h��ǉ����郁�\�b�h</summary>
    /// <param name="child">�ǉ�����q�m�[�h</param>
    public void AddChild(BaseNode child) => _children.Add(child);

    /// <summary>�m�[�h�̕]�������s���郁�\�b�h</summary>
    /// <returns>�m�[�h�̕]�����ʂ�Ԃ�</returns>
    public override NodeStatus Execute()
    {
        // �q�m�[�h�����ɕ]�����Ă����A�S�Ă̎q�m�[�h�����������ꍇ�ɐ����̕]�����ʂ�Ԃ�
        foreach (var child in _children)
        {
            NodeStatus status = child.Execute();

            if (status == NodeStatus.Failure)
            {
                return NodeStatus.Failure;
            }
            else if (status == NodeStatus.Running)
            {
                return NodeStatus.Running;
            }
        }
        return NodeStatus.Success;
    }
}
