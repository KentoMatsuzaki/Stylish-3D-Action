/// <summary>���N���X</summary>
public abstract class State
{
    protected Player _player;

    public State(Player player)
    {
        this._player = player;
    }

    /// <summary>�X�e�[�g�ɑJ�ڂ����ۂɌĂ΂��</summary>
    public abstract void Enter();

    /// <summary>�X�e�[�g����J�ڂ���ۂɌĂ΂��</summary>
    public abstract void Exit();
}

/// <summary>������</summary>
public class IdleState : State
{
    public IdleState(Player player) : base(player) { }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }
}

/// <summary>������</summary>
public class TrotState : State
{
    public TrotState(Player player) : base(player) { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}

/// <summary>�X�v�����g</summary>
public class SprintState : State
{
    public SprintState(Player player) : base(player) { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}

/// <summary>�W�����v</summary>
public class JumpState : State
{
    public JumpState(Player player) : base(player) { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}

/// <summary>�U��</summary>
public class AttackState : State
{
    public AttackState(Player player) : base(player) { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}

/// <summary>��_���[�W</summary>
public class DamageState : State
{
    public DamageState(Player player) : base(player) { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}

/// <summary>���S</summary>
public class DeadState : State
{
    public DeadState(Player player) : base(player) { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}