using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTemp : MonoBehaviour
{
    // reference variables
    PlayerInput _playerInput;
    CharacterController _characterController;
    Animator _animator;
    Camera _playerCamera;
    MouseLook _mouseLook;

    // variables to store player input values
    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector2 _currentMouseInput;
    bool _isMovementPressed;
    float _cameraRotationX = 0f;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _mouseLook = GetComponent<MouseLook>();

        _playerInput.CharacterControls.Move.started += OnMovementInput;
        _playerInput.CharacterControls.Move.canceled += OnMovementInput;

        _playerInput.CharacterControls.MouseX.performed += context => _currentMouseInput.x = context.ReadValue<float>();
        _playerInput.CharacterControls.MouseY.performed += context => _currentMouseInput.y = context.ReadValue<float>();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x;
        _currentMovement.z = _currentMovementInput.y;
        _isMovementPressed = _currentMovementInput.x != 0 || _currentMovementInput.y != 0;
    }

    void HandleAnimation()
    {
        bool isWalking = _animator.GetBool("isWalking");
        bool isRunning = _animator.GetBool("isRunning");

        if(_isMovementPressed && !isWalking)
        {
            _animator.SetBool("isWalking", true);
        }
        else if( !_isMovementPressed && isWalking)
        {
            _animator.SetBool("isWalking", false);
        }
    }
    
    private void Update()
    {
        HandleAnimation();
        _mouseLook.ReceiveInput(_currentMouseInput);

        _currentMovement = (transform.TransformDirection(Vector3.forward) * _currentMovementInput.x) + (transform.TransformDirection(Vector3.right) * _currentMovementInput.y);
        _characterController.Move(_currentMovement * Time.deltaTime);
    }

    private void OnEnable()
    {
        _playerInput.CharacterControls.Enable();
    }

    private void OnDisable()
    {
        _playerInput.CharacterControls.Disable();
    }

    
}
