public abstract class DraugrBaseState
{
    private bool _isRootState = false;
    private DraugrStateMachine _ctx;
    private DraugrStateLibrary _library;
    private DraugrBaseState _currentSubState;
    private DraugrBaseState _currentSuperState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected DraugrStateMachine Ctx { get { return _ctx; } }
    protected DraugrStateLibrary Library { get { return _library; } }

    public DraugrBaseState(DraugrStateMachine currentContext, DraugrStateLibrary playerStateFactory)
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

    protected void SwitchState(DraugrBaseState newState)
    {
        Exit();
        newState.Enter();

        if (_isRootState)
            _ctx.CurrentState = newState;
        else if (_currentSuperState != null)
            _currentSuperState.SetSubState(newState);
    }

    protected void SetSuperState(DraugrBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(DraugrBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
