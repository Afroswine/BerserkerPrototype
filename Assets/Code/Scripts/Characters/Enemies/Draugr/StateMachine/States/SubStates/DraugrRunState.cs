using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrRunState : DraugrBaseState
{
    public DraugrRunState(DraugrStateMachine currentContext, DraugrStateLibrary draugrStateLibrary)
        : base(currentContext, draugrStateLibrary) { }

    public override void Enter()
    {

    }

    public override void Tick()
    {

        CheckSwitchStates();
    }

    public override void Exit()
    {

    }

    public override void CheckSwitchStates()
    {

    }

    public override void InitializeSubState()
    {

    }
}
