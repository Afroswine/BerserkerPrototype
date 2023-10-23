using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraugrApproachTargetState : DraugrBaseState
{
    public DraugrApproachTargetState(DraugrStateMachine currentContext, DraugrStateLibrary draugrStateLibrary)
        : base(currentContext, draugrStateLibrary) { }

    GameObject _targetGO;

    public override void Enter()
    {

    }

    public override void Tick()
    {
        if (!_targetGO)
            _targetGO = FindTarget(Ctx.DraugrAgent);

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

    GameObject FindTarget(DraugrAgent agent)
    {
        if (agent.AISensor.VisibleTargets.Count == 0)
            return null;

        return agent.AISensor.VisibleTargets[0];
    }
}
