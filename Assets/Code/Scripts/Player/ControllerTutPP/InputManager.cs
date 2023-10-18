using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerMovement _movement;
    [SerializeField] MouseLook _mouseLook;
    
    PlayerInput _input;
    PlayerInput.GroundMovementActions _groundMovement;

    Vector2 _horizontalInput;
    Vector2 _mouseInput;

    private void Awake()
    {
        _input = new PlayerInput();
        _groundMovement = _input.GroundMovement;

        _groundMovement.HorizontalMovement.performed += context => _horizontalInput = context.ReadValue<Vector2>();
        
        _groundMovement.Jump.performed += _ => _movement.OnJumpPressed();

        _groundMovement.MouseX.performed += context => _mouseInput.x = context.ReadValue<float>();
        _groundMovement.MouseY.performed += context => _mouseInput.y = context.ReadValue<float>();
    }

    private void Update()
    {
        _movement.ReceiveInput(_horizontalInput);
        _mouseLook.ReceiveInput(_mouseInput);
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
