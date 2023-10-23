using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrFallState : DraugrBaseState
{
    public DraugrFallState(DraugrStateMachine currentContext, DraugrStateLibrary draugrStateLibrary)
        : base(currentContext, draugrStateLibrary)
    {
        IsRootState = true;
    }

    public override void Enter()
    {
        //Debug.Log(this.GetType().ToString() + ": Enter...");
        InitializeSubState();

        Ctx.Animator.SetBool(Ctx.IsFallingHash, true);
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

    }

    public override void InitializeSubState()
    {

    }

    public void HandleGravity()
    {

    }
}
