/// <summary></summary>
public abstract class State
{
    public abstract void Enter();
    public abstract void Exit();
}

/// <summary></summary>
public abstract class IdleState : State
{
    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }
}

/// <summary></summary>
public abstract class TrotState : State
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}

/// <summary></summary>
public abstract class SpintState : State
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}

/// <summary></summary>
public abstract class JumpState : State
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}

/// <summary></summary>
public abstract class AttackState : State
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}

/// <summary></summary>
public abstract class DamageState : State
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}

/// <summary></summary>
public abstract class DeadState : State
{
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }
}