using System.Collections.Generic;

// TODO - not really a factory anymore, maybe rename to "PlayerStateReferences" ?
public class PlayerStateLibrary
{
    private enum States
    {
        // super states
        grounded,
        jump,
        fall,

        // sub states
        idle,
        run,
        sprint
    }

    PlayerStateMachine _context;
    Dictionary<States, PlayerBaseState> _states = new Dictionary<States, PlayerBaseState>();

    public PlayerStateLibrary(PlayerStateMachine currentContext)
    {
        _context = currentContext;

        _states[States.grounded] = new PlayerGroundedState(_context, this);
        _states[States.jump] = new PlayerJumpState(_context, this);
        _states[States.fall] = new PlayerFallState(_context, this);

        _states[States.idle] = new PlayerIdleState(_context, this);
        _states[States.run] = new PlayerRunState(_context, this);
        _states[States.sprint] = new PlayerSprintState(_context, this);
    }

    // functions that return corresponding states
    // Super States (Parent States)
    public PlayerBaseState Grounded() { return _states[States.grounded]; }
    public PlayerBaseState Jump() { return _states[States.jump]; }
    public PlayerBaseState Fall() { return _states[States.fall]; }

    // Sub States (Child States)
    public PlayerBaseState Idle() { return _states[States.idle]; }
    public PlayerBaseState Run() { return _states[States.run]; }
    public PlayerBaseState Sprint() { return _states[States.sprint]; }
    
}
