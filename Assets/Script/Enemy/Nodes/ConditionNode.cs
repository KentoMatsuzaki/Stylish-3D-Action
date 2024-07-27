using System;

/// <summary>条件ノード</summary>
public class ConditionNode : BaseNode
{
    /// <summary>条件の評価結果を返すデリゲート</summary>
    private readonly Func<bool> _condition;

    /// <summary>コンストラクタ</summary>
    public ConditionNode(Func<bool> condition) => _condition = condition;

    /// <summary>ノードの評価を実行するメソッド</summary>
    /// <returns>ノードの評価結果を返す</returns>
    public override NodeStatus Execute()
        => _condition() ? NodeStatus.Success : NodeStatus.Failure;
}
