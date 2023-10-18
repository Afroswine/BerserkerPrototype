using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementNew : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] float _walkSpeed = 5.0f;
    [SerializeField] float _sprintSpeed = 8.0f;
    [SerializeField] float _slopeSlideSpeed = 8f;
    float _currentSpeed;

    [Header("Jumping Parameters")]
    [SerializeField] float _jumpHeight = 3.0f;
    [SerializeField] float _gravity = 30.0f;
    [SerializeField] float _coyoteTime = 0.2f;
    float _coyoteTimeCounter = 0f;
    bool _coyoteTimeEnabled;

    // component references
    private CharacterController _characterController;
    private Animator _animator;

    // input references
    PlayerInput _input;
    PlayerInput.GroundMovementActions _groundMovementInput;

    private Vector3 _moveDirection; // final move-amount applied to Character Controller
    private Vector2 _moveInput;   // WASD input values given to the Character Controller
    bool _isMovePressed;
    bool _isSprintPressed;
    bool _isJumpPressed = false;

    // animator variables
    int _isWalkingHash;
    int _isRunningHash;
    int _isJumpingHash;
    bool _isJumpAnimating;

    private void Awake()
    {
        // cache components
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        _isWalkingHash = Animator.StringToHash("isMoving");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");

        _input = new PlayerInput();
        _groundMovementInput = _input.GroundMovement;

        _groundMovementInput.HorizontalMovement.started += OnMovementInput;
        _groundMovementInput.HorizontalMovement.canceled += OnMovementInput;
        _groundMovementInput.HorizontalMovement.performed += OnMovementInput;
        _groundMovementInput.Sprint.started += OnSprint;
        _groundMovementInput.Sprint.canceled += OnSprint;
        _groundMovementInput.Jump.started += OnJump;
        _groundMovementInput.Jump.canceled += OnJump;
    }

    private void Start()
    {
        _gravity = -Mathf.Abs(_gravity);
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
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    private void Update()
    {
        HandleAnimation();
        HandleMovementInput();
        _characterController.Move(_moveDirection * Time.deltaTime);

        HandleGravity();
        HandleJump();
    }

    void HandleAnimation()
    {
        bool isWalking = _animator.GetBool(_isWalkingHash);
        bool isRunning = _animator.GetBool(_isRunningHash);

        if (_isMovePressed && !isWalking)
            _animator.SetBool(_isWalkingHash, true);
        else if (!_isMovePressed && isWalking)
            _animator.SetBool(_isWalkingHash, false);

        if ((_isMovePressed && _isSprintPressed) && !isRunning)
            _animator.SetBool(_isRunningHash, true);
        else if((!_isMovePressed || !_isSprintPressed) && isRunning)
            _animator.SetBool(_isRunningHash, false);
    }
    
    private void HandleGravity()
    {
        if (!_characterController.isGrounded)
        {
            _moveDirection.y += _gravity * Time.deltaTime;
        }
        else
        {
            _animator.SetBool(_isJumpingHash, false);
            _isJumpAnimating = false;

            if (_moveDirection.y < 0)
                _moveDirection.y = 0;

            //_coyoteTimeEnabled = true;
        }
    }

    // TODO - speed shouldn't be set in here...
    private void HandleMovementInput()
    {
        float speed = _isSprintPressed ? _sprintSpeed : _walkSpeed;
        float moveDirectionY = _moveDirection.y;

        _moveDirection = (transform.TransformDirection(Vector3.right) * _moveInput.x * speed) 
                       + (transform.TransformDirection(Vector3.forward) * _moveInput.y * speed);
        _moveDirection.y = moveDirectionY;
    }

    private void HandleJump()
    {
        if(_characterController.isGrounded && _isJumpPressed)
        {
            _animator.SetBool(_isJumpingHash, true);
            _isJumpAnimating = true;
            _moveDirection.y = Mathf.Sqrt(2f * _jumpHeight * -_gravity);
        }
    }
}

/*
    public void ReceiveInput(Vector2 moveInput, bool isSprintPressed)
    {
        //_moveInput = moveInput;
        //_isSprintPressed = isSprintPressed;

        //_isMovePressed = _moveInput.x != 0 || _moveInput.y != 0;
    }
*/