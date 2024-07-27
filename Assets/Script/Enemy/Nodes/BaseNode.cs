/// <summary>BehaviourTreeを構成するノードのベースクラス</summary>
public abstract class BaseNode
{
    /// <summary>ノードの評価を実行するメソッド</summary>
    /// <returns>ノードの評価結果を返す</returns>
    public abstract NodeStatus Execute();
}

/// <summary>ノードの評価結果を表す列挙型</summary>
public enum NodeStatus
{
    /// <summary>ノードの評価が成功したことを示す</summary>
    Success,

    /// <summary>ノードの評価が失敗したことを示す</summary>
    Failure,

    /// <summary>ノードの評価が実行中であることを示す</summary>
    Running
}
