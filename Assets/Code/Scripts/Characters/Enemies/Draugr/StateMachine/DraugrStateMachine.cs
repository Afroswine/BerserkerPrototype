using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DraugrStateMachine : MonoBehaviour
{
    [Header("Enemy State Machine")]
    [Header("Movement Parameters")]
    [SerializeField] float _runSpeed = 5f;
    [SerializeField] float _approachDistance = 2f;
    float _currentMoveSpeed;

    [Header("Gravity")]
    [SerializeField] float _gravity = 30.0f;
    [SerializeField] float _groundedGravity = 0.5f;

    NavMeshAgent _navMeshAgent;
    Animator _animator;

    DraugrBaseState _currentState;
    DraugrStateLibrary _states;

    public float RunSpeed => _runSpeed;
    public float ApproachDistance => _approachDistance;
    public float CurrentMoveSpeed { get { return _currentMoveSpeed; } set { _currentMoveSpeed = value; } }
    public float Gravity { get { return -Mathf.Abs(_gravity); } }
    public float GroundedGravity { get { return -Mathf.Abs(_groundedGravity); } }

    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public Animator Animator => _animator;

    public DraugrBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    private void Awake()
    {
        // setup state
        _states = new DraugrStateLibrary(this);
        //_currentState = // default state  // TODO
        //_currentState.Enter();    // TODO
    }

    private void Update()
    {
        //_currentState.TickSubStates();    // TODO
    }
}