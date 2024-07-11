using UnityEngine;

/// <summary>プレイヤーの状態の基底クラス</summary>
public abstract class State
{
    /// <summary>プレイヤーの制御クラス</summary>
    protected Player _player;

    /// <summary>コンストラクタ</summary>
    public State(Player player)
    {
        this._player = player;
    }

    /// <summary>ステートに遷移した際に呼ばれる</summary>
    public abstract void Enter();

    /// <summary>ステートから遷移する際に呼ばれる</summary>
    public abstract void Exit();
}

/// <summary>無操作</summary>
public class IdleState : State
{
    public IdleState(Player player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("現在のステート：Idle");

        // アニメーションを再生する
        _player.gameObject.GetComponent<Animator>().Play("Idle");
    }

    public override void Exit()
    {

    }
}

/// <summary>小走り</summary>
public class TrotState : State
{
    public TrotState(Player player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("現在のステート：Trot");
    }

    public override void Exit()
    {

    }
}

/// <summary>スプリント</summary>
public class SprintState : State
{
    public SprintState(Player player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("現在のステート：Sprint");
    }

    public override void Exit()
    {

    }
}

/// <summary>ジャンプ</summary>
public class JumpState : State
{
    public JumpState(Player player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("現在のステート：Jump");
    }

    public override void Exit()
    {

    }
}

/// <summary>攻撃</summary>
public class AttackState : State
{
    public AttackState(Player player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("現在のステート：Attack");
    }

    public override void Exit()
    {

    }
}

/// <summary>被ダメージ</summary>
public class DamageState : State
{
    public DamageState(Player player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("現在のステート：Damage");
    }

    public override void Exit()
    {

    }
}

/// <summary>死亡</summary>
public class DeadState : State
{
    public DeadState(Player player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("現在のステート：Dead");
    }

    public override void Exit()
    {

    }
}