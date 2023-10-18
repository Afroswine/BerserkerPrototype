using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void Enter()
    {
        Debug.Log(this.GetType().ToString() + ": Enter...");
        HandleJump();
    }

    public override void Tick()
    {
        CheckSwitchStates();
        HandleGravity();
    }

    public override void Exit()
    {
        _ctx.Animator.SetBool(_ctx.IsJumpingHash, false);
        _ctx.RequireNewJumpPress = true;
        Debug.Log(this.GetType().ToString() + ": Exit...");
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.CharacterController.isGrounded)
            SwitchState(_factory.Grounded());
    }

    public override void InitializeSubState()
    {

    }

    void HandleJump()
    {
        _ctx.Animator.SetBool(_ctx.IsJumpingHash, true);
        _ctx.MoveDirectionY = Mathf.Sqrt(2f * _ctx.JumpHeight * -_ctx.Gravity);
    }

    void HandleGravity()
    {
        _ctx.MoveDirectionY += _ctx.Gravity * Time.deltaTime;
    }
}
