using UnityEngine;

/// <summary>�v���C���[�̏�Ԃ̊��N���X</summary>
public abstract class State
{
    /// <summary>�v���C���[�̐���N���X</summary>
    protected Player _player;

    /// <summary>�R���X�g���N�^</summary>
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
        Debug.Log("���݂̃X�e�[�g�FIdle");

        // �A�j���[�V�������Đ�����
        _player.gameObject.GetComponent<Animator>().Play("Idle");
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
        Debug.Log("���݂̃X�e�[�g�FTrot");
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
        Debug.Log("���݂̃X�e�[�g�FSprint");
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
        Debug.Log("���݂̃X�e�[�g�FJump");
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
        Debug.Log("���݂̃X�e�[�g�FAttack");
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
        Debug.Log("���݂̃X�e�[�g�FDamage");
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
        Debug.Log("���݂̃X�e�[�g�FDead");
    }

    public override void Exit()
    {

    }
}