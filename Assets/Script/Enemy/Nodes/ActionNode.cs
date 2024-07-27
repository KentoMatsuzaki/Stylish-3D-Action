using System;

/// <summary>アクションノード</summary>
public class ActionNode : BaseNode
{
    /// <summary>ノードの評価結果を返すデリゲート</summary>
    private readonly Func<NodeStatus> _action;

    /// <summary>コンストラクタ</summary>
    public ActionNode(Func<NodeStatus> action) => _action = action;

    /// <summary>ノードの評価を実行するメソッド</summary>
    /// <returns>ノードの評価結果を返す</returns>
    public override NodeStatus Execute() => _action();
}
