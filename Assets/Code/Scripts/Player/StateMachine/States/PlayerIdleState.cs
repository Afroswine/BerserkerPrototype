using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory) 
        : base(currentContext, playerStateFactory) { }
    
    public override void Enter()
    {
        //Debug.Log(this.GetType().ToString() + ": Enter...");
        Ctx.Animator.SetBool(Ctx.IsMovingHash, false);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
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
        if(Ctx.IsMovePressed && Ctx.IsSprintPressed)
            SwitchState(Factory.Run());
        else if(Ctx.IsMovePressed && !Ctx.IsSprintPressed)
            SwitchState(Factory.Walk());
    }

    public override void InitializeSubState()
    {

    }
}
