using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] float _speed = 11f;
    [SerializeField] float _jumpHeight = 3f;
    [SerializeField] float _gravity = -30f;
    [SerializeField] LayerMask _groundMask;
    
    CharacterController _characterController;

    Vector2 _horizontalInput;
    Vector3 _verticalVelocity = Vector3.zero;
    bool _isGrounded;
    bool _jumpPressed;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // ground check
        _isGrounded = Physics.CheckSphere(transform.position, 0.1f, _groundMask);
        if (_isGrounded)
            _verticalVelocity.y = 0;
        
        // apply movement
        Vector3 horizontalVelocity = (transform.right * _horizontalInput.x + transform.forward * _horizontalInput.y) * _speed;
        _characterController.Move(horizontalVelocity * Time.deltaTime);

        // jumping
        if (_jumpPressed)
        {
            if (_isGrounded)
            {
                _verticalVelocity.y = Mathf.Sqrt(-2f * _jumpHeight * _gravity);
            }
            _jumpPressed = false;
        }
        

        // apply gravity
        _verticalVelocity.y += _gravity * Time.deltaTime;
        _characterController.Move(_verticalVelocity * Time.deltaTime);
    }

    public void ReceiveInput(Vector2 horizontalInput)
    {
        _horizontalInput = horizontalInput;
    }

    public void OnJumpPressed()
    {
        _jumpPressed = true;
    }
}
