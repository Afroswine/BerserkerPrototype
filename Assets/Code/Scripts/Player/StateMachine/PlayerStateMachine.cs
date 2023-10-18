using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Player State Machine")]
    [Header("Movement Parameters")]
    [SerializeField] float _walkSpeed = 5.0f;
    [SerializeField] float _sprintSpeed = 8.0f;
    [SerializeField] float _slopeSlideSpeed = 8f;
    float _currentMoveSpeed;
    public float WalkSpeed => _walkSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float CurrentMoveSpeed { get { return _currentMoveSpeed; } set { _currentMoveSpeed = value; } }

    [Header("Jumping Parameters")]
    [SerializeField] float _jumpHeight = 3.0f;
    [SerializeField] float _gravity = 30.0f;
    [SerializeField] float _groundedGravity = 0.5f;
    public float JumpHeight => _jumpHeight;
    public float Gravity { get { return -Mathf.Abs(_gravity); } }
    public float GroundedGravity { get { return -Mathf.Abs(_groundedGravity); } }

    // component references
    CharacterController _characterController;
    Animator _animator;
    PlayerInput _input;
    PlayerInput.GroundMovementActions _groundMovementInput;
    public CharacterController CharacterController => _characterController;
    public Animator Animator => _animator;

    Vector2 _moveInput;   // WASD input values given to the Character Controller
    Vector3 _moveDirection; // final move-amount applied to Character Controller
    bool _requireNewJumpPress = false;
    public float MoveDirectionY { get { return _moveDirection.y; } set { _moveDirection.y = value; } }
    public bool RequireNewJumpPress { get { return _requireNewJumpPress; } set { _requireNewJumpPress = value; } }

    bool _isMovePressed;
    bool _isSprintPressed;
    bool _isJumpPressed;
    public bool IsMovePressed => _isMovePressed;
    public bool IsSprintPressed => _isSprintPressed;
    public bool IsJumpPressed => _isJumpPressed;

    // animator variables
    int _isMovingHash;
    int _isRunningHash;
    int _isJumpingHash;
    int _isFallingHash;
    public int IsMovingHash => _isMovingHash;
    public int IsRunningHash => _isRunningHash;
    public int IsJumpingHash => _isJumpingHash;
    public int IsFallingHash => _isFallingHash;

    // state variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    
    private void Awake()
    {
        // reference variables
        _input = new PlayerInput();
        _groundMovementInput = _input.GroundMovement;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        // setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.Enter();

        // animator hash references
        _isMovingHash = Animator.StringToHash("isMoving");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _isFallingHash = Animator.StringToHash("isFalling");

        // input callbacks
        _groundMovementInput.HorizontalMovement.started += OnMovementInput;
        _groundMovementInput.HorizontalMovement.canceled += OnMovementInput;
        _groundMovementInput.HorizontalMovement.performed += OnMovementInput;
        _groundMovementInput.Sprint.started += OnSprint;
        _groundMovementInput.Sprint.canceled += OnSprint;
        _groundMovementInput.Jump.started += OnJump;
        _groundMovementInput.Jump.canceled += OnJump;
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        _isMovePressed = _moveInput.x != 0 || _moveInput.y != 0;
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        _isSprintPressed = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        _requireNewJumpPress = false;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    // TODO - should _charactercontroller.Move() be called here instead of inside a state?
    private void Update()
    {
        _currentState.TickSubStates();
        HandleMovementInput();
        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    // TODO - movement should probably have its own script(s) that other states reference through the StateMachine (context)
    //        this method for instance could return _moveDirection as a Vector3. But should it be applied in the super-state or sub-state?
    private void HandleMovementInput()
    {
        float moveDirectionY = _moveDirection.y;
        _moveDirection = (transform.TransformDirection(Vector3.right) * _moveInput.x * _currentMoveSpeed)
                       + (transform.TransformDirection(Vector3.forward) * _moveInput.y * _currentMoveSpeed);
        _moveDirection.y = moveDirectionY;
    }
}
