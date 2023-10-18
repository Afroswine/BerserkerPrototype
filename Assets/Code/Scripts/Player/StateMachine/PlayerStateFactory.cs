using System.Collections.Generic;

enum PlayerStates
{
    // super states
    grounded,
    jump,
    fall,

    // sub states
    idle,
    walk,
    run
}

// TODO - not really a factory anymore, maybe rename to "PlayerStateReferences" ?
public class PlayerStateFactory
{
    PlayerStateMachine _context;
    Dictionary<PlayerStates, PlayerBaseState> _states = new Dictionary<PlayerStates, PlayerBaseState>();

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;

        _states[PlayerStates.grounded] = new PlayerGroundedState(_context, this);
        _states[PlayerStates.jump] = new PlayerJumpState(_context, this);
        _states[PlayerStates.fall] = new PlayerFallState(_context, this);

        _states[PlayerStates.idle] = new PlayerIdleState(_context, this);
        _states[PlayerStates.walk] = new PlayerWalkState(_context, this);
        _states[PlayerStates.run] = new PlayerRunState(_context, this);
    }

    // Super States (Parent States)
    public PlayerBaseState Grounded() { return _states[PlayerStates.grounded]; }
    public PlayerBaseState Jump() { return _states[PlayerStates.jump]; }
    public PlayerBaseState Fall() { return _states[PlayerStates.fall]; }

    // Sub States (Child States)
    public PlayerBaseState Idle() { return _states[PlayerStates.idle]; }
    public PlayerBaseState Walk() { return _states[PlayerStates.walk]; }
    public PlayerBaseState Run() { return _states[PlayerStates.run]; }
    
}
