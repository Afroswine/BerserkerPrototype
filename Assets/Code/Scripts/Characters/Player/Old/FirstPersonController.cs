using System.Collections;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    //Is the player in control of the character right now? (defaulted to True)
    public bool CanMove { get; private set; } = true;
    private bool IsSprinting => _canSprint && Input.GetKey(_sprintKey);
    private bool ShouldJump => Input.GetKeyDown(_jumpKey) && (_characterController.isGrounded || _coyoteTimeCounter > 0);
    private bool ShouldCrouch => Input.GetKeyDown(_crouchKey) && !_duringCrouchAnimation; //&& _characterController.isGrounded;

    [Header("Functional Options")]
    [SerializeField] bool _canSprint = true;
    [SerializeField] bool _canJump = true;
    [SerializeField] bool _canCrouch = true;
    [SerializeField] bool _canUseHeadbob = true;
    [SerializeField] bool _willSlideOnSlopes = true;

    [Header("Controls")]
    [SerializeField] KeyCode _sprintKey = KeyCode.LeftShift;
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] KeyCode _crouchKey = KeyCode.LeftControl;

    [Header("Movement Parameters")]
    [SerializeField] float _walkSpeed = 5.0f;
    [SerializeField] float _sprintSpeed = 8.0f;
    [SerializeField] float _slopeSlideSpeed = 8f;

    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] float _lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] float _lookSpeedY = 2.0f;
    [SerializeField, Range(1, 100)] float _upperLookLimit = 80f;
    [SerializeField, Range(1, 100)] float _lowerLookLimit = 80f;
    Vector3 _cameraStandingPosition;

    [Header("Jumping Parameters")]
    [SerializeField] float _jumpHeight = 3.0f;
    [SerializeField] float _gravity = 30.0f;
    [SerializeField] float _coyoteTime = 0.2f;
    float _coyoteTimeCounter = 0f;
    bool _coyoteTimeEnabled;

    [Header("Crouch Parameters")]
    [SerializeField] float _crouchSpeed = 2.5f;
    [SerializeField] float _crouchHeight = 1f;
    [SerializeField] float _cameraCrouchYPosition = 0.75f; // new
    [SerializeField] float _timeToCrouch = 0.25f;
    float _standingHeight;
    Vector3 _standingCenter;
    Vector3 _crouchCenter;
    Vector3 _cameraCrouchPosition;
    bool _isCrouching = false;
    bool _duringCrouchAnimation = false;

    [Header("Headbob Parameters")]
    [SerializeField] float _walkBobSpeed = 14f;
    [SerializeField] float _walkBobAmount = 0.05f;
    [SerializeField] float _sprintBobSpeed = 18f;
    [SerializeField] float _sprintBobAmount = 0.1f;
    [SerializeField] float _crouchBobSpeed = 8f;
    [SerializeField] float _crouchBobAmount = 0.025f;
    float _headbobTimer;

    // SLOPE SLIDING PARAMETERS
    Vector3 _hitPointNormal;
    bool IsSliding
    {
        get
        {
            // if the player is grounded but the surface's slope is higher than the CC's slope limit...
            if (_characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 1f))
            {
                _hitPointNormal = slopeHit.normal;
                return Vector3.Angle(_hitPointNormal, Vector3.up) > _characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    // component references
    private Camera _playerCamera;
    private CharacterController _characterController;

    private Vector3 _moveDirection; // final move-amount applied to Character Controller
    private Vector2 _currentMovementInput;   // WASD input values given to the Character Controller

    private float _cameraRotationX = 0f;

    private void Awake()
    {
        // cache components
        _playerCamera = GetComponentInChildren<Camera>();
        _characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        // store default camera position
        _cameraStandingPosition = _playerCamera.transform.localPosition;

        // set crouching variables
        _standingHeight = _characterController.height;
        _standingCenter = _characterController.center;
        _crouchCenter = new Vector3(0, _crouchHeight / 2, 0);
        _cameraCrouchPosition = new Vector3(0, _cameraCrouchYPosition, 0);


        // lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // TODO - this is a lot of if statements inside of update, consider using unity events instead.
        if (CanMove)
        {
            HandleMovementInput();
            HandleMouseLook();

            if (_canJump)
                HandleJump();

            if (_canCrouch)
                HandleCrouch();

            if (_canUseHeadbob)
                HandleHeadbob();

            ApplyFinalMovements();
        }
    }

    private void HandleMovementInput()
    {
        // store inputs and multiply them by speed values
        _currentMovementInput = new Vector2((_isCrouching ? _crouchSpeed : IsSprinting ? _sprintSpeed : _walkSpeed) * Input.GetAxis("Vertical"),
            (_isCrouching ? _crouchSpeed : IsSprinting ? _sprintSpeed : _walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = _moveDirection.y;
        _moveDirection = (transform.TransformDirection(Vector3.forward) * _currentMovementInput.x) + (transform.TransformDirection(Vector3.right) * _currentMovementInput.y);
        _moveDirection = Vector3.ClampMagnitude(_moveDirection, _isCrouching ? _crouchSpeed : IsSprinting ? _sprintSpeed : _walkSpeed);
        _moveDirection.y = moveDirectionY;
    }

    private void HandleMouseLook()
    {
        // looking up and down (rotates camera)
        _cameraRotationX -= Input.GetAxis("Mouse Y") * _lookSpeedY;
        _cameraRotationX = Mathf.Clamp(_cameraRotationX, -_upperLookLimit, _lowerLookLimit);
        _playerCamera.transform.localRotation = Quaternion.Euler(_cameraRotationX, 0, 0);

        // looking left and right (rotates parent game object)
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if (ShouldJump && !IsSliding)
            _moveDirection.y = Mathf.Sqrt(2f * _jumpHeight * _gravity);
    }

    private void HandleCrouch()
    {
        if (ShouldCrouch)
            StartCoroutine(CrouchStand());
    }

    private void HandleHeadbob()
    {
        // only headbob while grounded
        if (!_characterController.isGrounded) return;

        // TODO - try to remove if statements
        if (Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.z) > 0.1f)
        {
            _headbobTimer += Time.deltaTime * (_isCrouching ? _crouchBobSpeed : IsSprinting ? _sprintBobSpeed : _walkBobSpeed);
            _playerCamera.transform.localPosition = new Vector3(
                _playerCamera.transform.localPosition.x,
                (_isCrouching ? _cameraCrouchPosition.y : _cameraStandingPosition.y) + Mathf.Sin(_headbobTimer) * (_isCrouching ? _crouchBobAmount : IsSprinting ? _sprintBobAmount : _walkBobAmount),
                _playerCamera.transform.localPosition.z);
        }
    }

    private void ApplyFinalMovements()
    {
        // handle gravity and coyote time
        if (!_characterController.isGrounded)
        {
            _moveDirection.y -= _gravity * Time.deltaTime;

            if (_coyoteTimeEnabled)
                StartCoroutine(CoyoteTime());
        }
        else    // TODO - should this be set every frame?
        {
            if (_moveDirection.y < 0)
                _moveDirection.y = 0;

            _coyoteTimeEnabled = true;
        }

        // handle slopes
        if (_willSlideOnSlopes && IsSliding)
            _moveDirection += new Vector3(_hitPointNormal.x, -_hitPointNormal.y, _hitPointNormal.z) * _slopeSlideSpeed;

        // apply movement calculated in HandleMovementInput()
        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    private IEnumerator CrouchStand()
    {
        // if already crouching, raycast check above the player do deterine if they can uncrouch
        // TODO - the '1f' number may need to be changed for specific models.
        if (_isCrouching && Physics.Raycast(_playerCamera.transform.position, Vector3.up, 1f))
            yield break;
        
        _duringCrouchAnimation = true;

        float timeElapsed = 0;

        // character controller values
        float targetHeight = _isCrouching ? _standingHeight : _crouchHeight;
        float currentHeight = _characterController.height;
        Vector3 targetCenter = _isCrouching ? _standingCenter : _crouchCenter;
        Vector3 currentCenter = _characterController.center;

        // camera values
        // TODO - consider animating camera instead
        Vector3 targetCameraHeight = _isCrouching ? _cameraStandingPosition : _cameraCrouchPosition;
        Vector3 currentCameraHeight = _playerCamera.transform.localPosition;

        // lerp from current height to target height over time
        while (timeElapsed < _timeToCrouch)
        {
            // lerp character controller height & center
            _characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / _timeToCrouch);
            _characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / _timeToCrouch);

            // adjust camera position
            // TODO - consider animating camera instead
            _playerCamera.transform.localPosition = Vector3.Lerp(currentCameraHeight, targetCameraHeight, timeElapsed / _timeToCrouch);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // ensure targets are met at the end of the duration
        _characterController.height = targetHeight;
        _characterController.center = targetCenter;
        _playerCamera.transform.localPosition = targetCameraHeight;

        _isCrouching = !_isCrouching;
        _duringCrouchAnimation = false;
    }

    // affects how long the player can still jump after becoming not grounded
    private IEnumerator CoyoteTime()
    {
        _coyoteTimeCounter = _coyoteTime;

        while (_coyoteTimeCounter > 0)
        {
            _coyoteTimeCounter -= Time.deltaTime;
            yield return null;
        }

        _coyoteTimeCounter = 0;
        _coyoteTimeEnabled = false;
    }
}
