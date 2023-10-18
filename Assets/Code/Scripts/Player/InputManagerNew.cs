using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManagerNew : MonoBehaviour
{
    // component references
    PlayerMovementNew _playerMovement;
    MouseLook _mouseLook;
    
    // input references
    PlayerInput _input;
    PlayerInput.GroundMovementActions _groundMovementInput;

    // input variables
    Vector2 _moveInput;
    Vector2 _mouseInput;

    bool _isSprintPressed;

    private void Awake()
    {
        _playerMovement = GetComponent<PlayerMovementNew>();
        _mouseLook = GetComponent<MouseLook>();

        _input = new PlayerInput();
        _groundMovementInput = _input.GroundMovement;

        /*
        _groundMovementInput.HorizontalMovement.started += OnMovementInput;
        _groundMovementInput.HorizontalMovement.canceled += OnMovementInput;
        _groundMovementInput.HorizontalMovement.performed += OnMovementInput;
        _groundMovementInput.Sprint.started += OnSprint;
        _groundMovementInput.Sprint.canceled += OnSprint;
        _groundMovementInput.Jump.started += OnJump;
        _groundMovementInput.Jump.canceled += OnJump;
        */

        //_groundMovementInput.MouseX.performed += context => _mouseInput.x = context.ReadValue<float>();
        //_groundMovementInput.MouseY.performed += context => _mouseInput.y = context.ReadValue<float>();
    }

    private void Update()
    {
        //_playerMovement.ReceiveInput(_moveInput, _isSprintPressed);
        //_mouseLook.ReceiveInput(_mouseInput);
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        _isSprintPressed = context.ReadValueAsButton();
    }

    void OnJump(InputAction.CallbackContext context)
    {

    }
}
