using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrAttackState : DraugrBaseState
{
    public DraugrAttackState(DraugrStateMachine currentContext, DraugrStateLibrary draugrStateLibrary)
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
