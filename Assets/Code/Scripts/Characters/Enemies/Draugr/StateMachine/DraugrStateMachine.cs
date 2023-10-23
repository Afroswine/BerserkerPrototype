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

    public float RunSpeed => _runSpeed;
    public float ApproachDistance => _approachDistance;
    public float CurrentMoveSpeed { get { return _currentMoveSpeed; } set { _currentMoveSpeed = value; } }
    public float Gravity { get { return -Mathf.Abs(_gravity); } }
    public float GroundedGravity { get { return -Mathf.Abs(_groundedGravity); } }

    // component references
    NavMeshAgent _navMeshAgent;
    DraugrAgent _draugrAgent;
    Animator _animator;

    public NavMeshAgent NavMeshAgent => _navMeshAgent;
    public DraugrAgent DraugrAgent => _draugrAgent;
    public Animator Animator => _animator;

    // animation variables
    int _substateIDHash;
    int _substateChangeHash;
    public int SubstateIDHash => _substateIDHash;
    public int SubstateChangeHash => _substateChangeHash;

    Vector2 _smoothMoveInput = Vector2.zero;
    int _isMovingHash;
    int _isFallingHash;
    int _isCombatHash;
    int _isAttackingHash;
    int _smoothMoveXHash;
    int _smoothMoveYHash;
    public int IsMovingHash => _isMovingHash;
    public int IsFallingHash => _isFallingHash;
    public int IsCombatHash => _isCombatHash;
    public int IsAttackingHash => _isAttackingHash;
    public int MoveXHash => _smoothMoveXHash;
    public int MoveYHash => _smoothMoveYHash;

    // state-machine
    DraugrBaseState _currentState;
    DraugrStateLibrary _states;

    public DraugrBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }

    private void Awake()
    {
        // reference variables
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        
        // setup state
        _states = new DraugrStateLibrary(this);
        _currentState = _states.Grounded();
        _currentState.Enter();

        // animator hash references
        _substateIDHash = Animator.StringToHash("substateID");
        _substateChangeHash = Animator.StringToHash("substateChange");
        _isMovingHash = Animator.StringToHash("isMoving");
        _isFallingHash = Animator.StringToHash("isFalling");
        _isCombatHash = Animator.StringToHash("isCombat");
        _isAttackingHash = Animator.StringToHash("isAttacking");
        _smoothMoveXHash = Animator.StringToHash("smoothMoveX");
        _smoothMoveYHash = Animator.StringToHash("smoothMoveY");
    }

    private void Update()
    {
        _currentState.TickSubStates();
    }
}