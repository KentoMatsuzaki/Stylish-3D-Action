using System.Collections.Generic;

/// <summary>セレクターノード</summary>
public class SelectorNode : BaseNode
{
    /// <summary>子ノードのリスト</summary>
    private readonly List<BaseNode> _children = new List<BaseNode>();

    /// <summary>子ノードのリストに新たな子ノードを追加するメソッド</summary>
    /// <param name="child">追加する子ノード</param>
    public void AddChild(BaseNode child) => _children.Add(child);

    /// <summary>ノードの評価を実行するメソッド</summary>
    /// <returns>ノードの評価結果を返す</returns>
    public override NodeStatus Execute()
    {
        // 子ノードを順に評価していき、一つでも成功した場合に成功の評価結果を返す
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
