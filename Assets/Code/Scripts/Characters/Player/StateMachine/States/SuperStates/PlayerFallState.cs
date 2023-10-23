using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateLibrary playerStateLibrary)
        : base(currentContext, playerStateLibrary)
    {
        IsRootState = true;
    }
    
    public override void Enter()
    {
        InitializeSubState();

        Ctx.Animator.SetBool(Ctx.IsFallingHash, true);
    }

    public override void Tick()
    {
        HandleGravity();
        
        CheckSwitchStates();
    }

    public override void Exit()
    {
        Ctx.Animator.SetBool(Ctx.IsFallingHash, false);
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
    
    public void HandleGravity()
    {
        Ctx.MoveDirectionY += Ctx.Gravity * Time.deltaTime;
    }
    
}
