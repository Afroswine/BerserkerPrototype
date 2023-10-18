using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void Enter()
    {
        //Debug.Log(this.GetType().ToString() + ": Enter...");
        Ctx.Animator.SetBool(Ctx.IsMovingHash, true);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, true);
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
            SwitchState(Factory.Idle());
        else if (Ctx.IsMovePressed && !Ctx.IsSprintPressed)
            SwitchState(Factory.Walk());
    }

    public override void InitializeSubState()
    {

    }
}
