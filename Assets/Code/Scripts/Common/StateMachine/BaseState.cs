/*
public abstract class BaseState
{
    private bool _isRootState = false;
    private StateMachine _ctx;
    private StateLibrary _library;
    private BaseState _currentSubState;
    private BaseState _currentSuperState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected StateMachine Ctx { get { return _ctx; } }
    protected StateLibrary Library { get { return _library; } }

    public BaseState(StateMachine currentContext, StateLibrary stateFactory)
    {
        _ctx = currentContext;
        _library = stateFactory;
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

    public void ExitSubStates()
    {
        Exit();
        if (_currentSubState != null)
            _currentSubState.ExitSubStates();
    }
    

    protected void SwitchState(BaseState newState)
    {
        Exit();
        newState.Enter();

        if (_isRootState)
            _ctx.CurrentState = newState;
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState);
    }

    protected void SetSuperState(BaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(BaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
*/