using System.Collections.Generic;

public class DraugrStateLibrary
{
    private enum States
    {
        // super states
        grounded,
        fall,

        // sub states
        idle,
        run,
        approachTarget,
        attack
    }

    DraugrStateMachine _context;
    Dictionary<States, DraugrBaseState> _states = new Dictionary<States, DraugrBaseState>();

    public DraugrStateLibrary(DraugrStateMachine currentContext)
    {
        _context = currentContext;

        _states[States.grounded] = new DraugrGroundedState(_context, this);
        _states[States.fall] = new DraugrFallState(_context, this);

        _states[States.idle] = new DraugrIdleState(_context, this);
        _states[States.run] = new DraugrRunState(_context, this);
        _states[States.approachTarget] = new DraugrApproachTargetState(_context, this);
        _states[States.attack] = new DraugrAttackState(_context, this);
    }

    // Super States
    public DraugrBaseState Grounded() { return _states[States.grounded]; }
    public DraugrBaseState Fall() { return _states[States.fall]; }

    // Sub States
    public DraugrBaseState Idle() { return _states[States.idle]; }
    public DraugrBaseState Run() { return _states[States.run]; }
    public DraugrBaseState ApproachTarget() { return _states[States.approachTarget]; }
    public DraugrBaseState Attack() { return _states[States.attack]; }
}