using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void Enter()
    {
        //Debug.Log(this.GetType().ToString() + ": Enter...");
        Ctx.Animator.SetBool(Ctx.IsMovingHash, true);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
        Ctx.CurrentMoveSpeed = Ctx.WalkSpeed;
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
        else if (Ctx.IsMovePressed && Ctx.IsSprintPressed)
            SwitchState(Factory.Run());
    }

    public override void InitializeSubState()
    {

    }
}
