public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateLibrary _library;
    private PlayerBaseState _currentSubState;
    private PlayerBaseState _currentSuperState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected PlayerStateMachine Ctx { get { return _ctx; } }
    protected PlayerStateLibrary Library { get { return _library; } }

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateLibrary playerStateFactory)
    {
        _ctx = currentContext;
        _library = playerStateFactory;
    }
    
    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void TickSubStates()
    {
        Tick();
        if (_currentSubState != null)
            _currentSubState.TickSubStates();
    }

/*
    public void ExitSubStates()
    {
        Exit();
        if (_currentSubState != null)
            _currentSubState.ExitSubStates();
    }
*/

    protected void SwitchState(PlayerBaseState newState)
    {
        Exit();
        newState.Enter();

        if (_isRootState)
            _ctx.CurrentState = newState;
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState);
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
