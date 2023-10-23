using UnityEngine;

public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateLibrary playerStateLibrary)
        : base(currentContext, playerStateLibrary)
    {
        IsRootState = true;
        
    }

    public override void Enter()
    {
        //Debug.Log(this.GetType().ToString() + ": Enter...");
        InitializeSubState();

        HandleGravity();
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
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress)
            SwitchState(Library.Jump());
        else if (!Ctx.CharacterController.isGrounded)
            SwitchState(Library.Fall());
    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovePressed && !Ctx.IsSprintPressed)
            SetSubState(Library.Idle());
        else if (Ctx.IsMovePressed && !Ctx.IsSprintPressed)
            SetSubState(Library.Run());
        else if (Ctx.IsMovePressed && Ctx.IsSprintPressed)
            SetSubState(Library.Sprint());
    }

    
    public void HandleGravity()
    {
        Ctx.MoveDirectionY = Ctx.GroundedGravity;
    }
    
}
