using UnityEngine;

public class PlayerJumpState : PlayerBaseState, IRootState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateLibrary playerStateFactory)
        : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public override void Enter()
    {
        //Debug.Log(this.GetType().ToString() + ": Enter...");
        InitializeSubState();

        HandleJump();
    }

    public override void Tick()
    {
        HandleGravity();

        CheckSwitchStates();
    }

    public override void Exit()
    {
        Ctx.Animator.SetBool(Ctx.IsJumpingHash, false);
        Ctx.RequireNewJumpPress = true;
        //Debug.Log(this.GetType().ToString() + ": Exit...");
    }

    public override void CheckSwitchStates()
    {
        if (Ctx.CharacterController.isGrounded)
            SwitchState(Library.Grounded());
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

    
    void HandleJump()
    {
        Ctx.Animator.SetBool(Ctx.IsJumpingHash, true);
        Ctx.MoveDirectionY = Mathf.Sqrt(2f * Ctx.JumpHeight * -Ctx.Gravity);
    }

    public void HandleGravity()
    {
        Ctx.MoveDirectionY += Ctx.Gravity * Time.deltaTime;
    }
    
}