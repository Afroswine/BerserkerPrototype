using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateLibrary playerStateFactory) 
        : base(currentContext, playerStateFactory) { }
    
    public override void Enter()
    {
        //Debug.Log(this.GetType().ToString() + ": Enter...");
        Ctx.Animator.SetBool(Ctx.IsMovingHash, false);
        Ctx.Animator.SetBool(Ctx.IsSprintingHash, false);
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
            SwitchState(Library.Sprint());
        else if(Ctx.IsMovePressed && !Ctx.IsSprintPressed)
            SwitchState(Library.Run());
    }

    public override void InitializeSubState()
    {

    }
}
