/*
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    BaseState _currentState;
    StateLibrary _states;

    public BaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public StateLibrary States { get { return _states; } set { _states = value; } }

    private void Awake()
    {
        //_states = new StateFactory(this);
        //_currentState = _states.Default();
        //_currentState.Enter();
    }

    private void Update()
    {
        _currentState.TickSubStates();
    }
}
*/