using System.Collections.Generic;

/// <summary>�Z���N�^�[�m�[�h</summary>
public class SelectorNode : BaseNode
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
        // �q�m�[�h�����ɕ]�����Ă����A��ł����������ꍇ�ɐ����̕]�����ʂ�Ԃ�
        foreach (var child in _children)
        {
            NodeStatus status = child.Execute();

            if (status == NodeStatus.Success)
            {
                return NodeStatus.Success;
            }
            else if (status == NodeStatus.Running)
            {
                return NodeStatus.Running;
            }
        }
        return NodeStatus.Failure;
    }
}
