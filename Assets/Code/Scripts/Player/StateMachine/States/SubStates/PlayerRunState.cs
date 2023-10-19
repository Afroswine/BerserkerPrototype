using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateLibrary playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void Enter()
    {
        //Debug.Log(this.GetType().ToString() + ": Enter...");
        Ctx.Animator.SetBool(Ctx.IsMovingHash, true);
        Ctx.Animator.SetBool(Ctx.IsSprintingHash, false);
        Ctx.CurrentMoveSpeed = Ctx.RunSpeed;
    }

    public override void Tick()
    {
        
        CheckSwitchStates();
    }

    public override void Exit()
    {
        //Debug.Log(this.GetType().ToString() + ": Exit...");
    }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMovePressed)
            SwitchState(Library.Idle());
        else if (Ctx.IsMovePressed && Ctx.IsSprintPressed)
            SwitchState(Library.Sprint());
    }

    public override void InitializeSubState()
    {

    }
}
