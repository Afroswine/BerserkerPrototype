using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallState : PlayerBaseState, IRootState
{
    public PlayerFallState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory)
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
            SwitchState(Factory.Grounded());
    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovePressed && !Ctx.IsSprintPressed)
            SetSubState(Factory.Idle());
        else if (Ctx.IsMovePressed && !Ctx.IsSprintPressed)
            SetSubState(Factory.Walk());
        else if (Ctx.IsMovePressed && Ctx.IsSprintPressed)
            SetSubState(Factory.Run());
    }

    public void HandleGravity()
    {
        Ctx.MoveDirectionY += Ctx.Gravity * Time.deltaTime;
    }
}
