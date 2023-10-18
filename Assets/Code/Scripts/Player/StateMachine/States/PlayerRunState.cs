using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory) { }

    public override void Enter()
    {
        Debug.Log(this.GetType().ToString() + ": Enter...");
    }

    public override void Tick()
    {

    }

    public override void Exit()
    {
        Debug.Log(this.GetType().ToString() + ": Exit...");
    }

    public override void CheckSwitchStates()
    {

    }

    public override void InitializeSubState()
    {

    }
}
