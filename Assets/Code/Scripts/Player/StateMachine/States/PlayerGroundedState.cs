using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void Enter()
    {
        Debug.Log(this.GetType().ToString() + ": Enter...");
        _ctx.MoveDirectionY = _ctx.GroundedGravity;
    }

    public override void Tick()
    {
        CheckSwitchStates();
    }

    public override void Exit()
    {
        Debug.Log(this.GetType().ToString() + ": Exit...");
    }

    public override void CheckSwitchStates()
    {
        if (_ctx.IsJumpPressed && !_ctx.RequireNewJumpPress)
            SwitchState(_factory.Jump());
    }

    public override void InitializeSubState()
    {

    }
}
