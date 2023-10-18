using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Look")]
    [SerializeField] float _sensitivityX = 10f;
    [SerializeField] float _sensitivityY = 10f;
    [SerializeField] Camera _playerCamera;
    [SerializeField, Range(1, 100)] float _xClamp = 85f;

    float _mouseX, _mouseY;
    float _rotationX = 0f;

    PlayerInput _input; // NEW
    PlayerInput.GroundMovementActions _groundMovementInput; // NEW

    // NEW
    private void Awake()
    {
        _input = new PlayerInput();
        _groundMovementInput = _input.GroundMovement;

        _groundMovementInput.MouseX.performed += OnMouseX;
        _groundMovementInput.MouseY.performed += OnMouseY;
    }

    // NEW
    private void OnMouseX(InputAction.CallbackContext context)
    {
        _mouseX = context.ReadValue<float>();
    }
     
    // NEW
    private void OnMouseY(InputAction.CallbackContext context)
    {
        _mouseY = context.ReadValue<float>();
    }

    // NEW
    private void OnEnable()
    {
        _input.Enable();
    }

    // NEW
    private void OnDisable()
    {
        _input.Disable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMouseLook();
    }

    public void ReceiveInput(Vector2 mouseInput)
    {
        _mouseX = mouseInput.x;
        _mouseY = mouseInput.y;
    }

    private void HandleMouseLook()
    {
        // looking up and down (rotates camera)
        _rotationX -= _mouseY * _sensitivityY * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, -_xClamp, _xClamp);
        _playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);

        // looking left and right (rotates game object)
        transform.rotation *= Quaternion.Euler(0, _mouseX * _sensitivityX * Time.deltaTime, 0);
    }
}
