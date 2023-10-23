using UnityEngine;

public class PlayerSprintState : PlayerBaseState
{
    public PlayerSprintState(PlayerStateMachine currentContext, PlayerStateLibrary playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void Enter()
    {
        //Debug.Log(this.GetType().ToString() + ": Enter...");
        Ctx.Animator.SetBool(Ctx.IsMovingHash, true);
        Ctx.Animator.SetBool(Ctx.IsSprintingHash, true);
        Ctx.CurrentMoveSpeed = Ctx.SprintSpeed;
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
        else if (Ctx.IsMovePressed && !Ctx.IsSprintPressed)
            SwitchState(Library.Run());
    }

    public override void InitializeSubState()
    {

    }
}
