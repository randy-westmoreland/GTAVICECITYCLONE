using UnityEngine;

/// <summary>
/// Handles player movement functionalities.
/// </summary>
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(AnimatorManager))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Scripts References")]
    private InputManager _inputManager;
    private PlayerManager _playerManager;
    private AnimatorManager _animatorManager;

    public Transform _cameraObject;

    [Header("Movement Flags")]
    [SerializeField] private bool _isMoving;
    [SerializeField] private bool _isSprinting;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _isJumping;

    [Header("Movement Values")]
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private float _walkingSpeed = 1.5f;
    [SerializeField] private float _runningSpeed = 5f;
    [SerializeField] private float _sprintingSpeed = 7f;
    [SerializeField] private float _rotationSpeed = 12f;

    [Header("Falling and Landing")]
    [SerializeField] private float _inAirTimer;
    [SerializeField] private float _leapingVelocity;
    [SerializeField] private float _fallingVelocity;
    [SerializeField] private float _rayCastHeightOffset = 0.5f;
    [SerializeField] private LayerMask _groundLayer;  // May not need to be a serialized field

    [Header("Jumping Variables")]
    [SerializeField] private float _jumpHeight = 4f;
    [SerializeField] private float _gravityIntensity = -15f;

    private Rigidbody _playerRigidbody;

    /// <summary>
    /// Gets or sets a value indicating whether the player is sprinting.
    /// </summary>
    public bool IsSprinting { get => _isSprinting; set => _isSprinting = value; }

    /// <summary>
    /// Gets or sets a value indicating whether the player is jumping.
    /// </summary>
    public bool IsJumping { get => _isJumping; set => _isJumping = value; }

    /// <summary>
    /// Gets or sets a value indicating whether the player is grounded.
    /// </summary>
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }

    /// <summary>
    /// Gets or sets a value indicating whether the player is moving.
    /// </summary>
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    /// <summary>
    /// Handles all movement-related functionalities.
    /// </summary>
    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if (_playerManager.IsInteracting)
        {
            return;
        }

        HandleMovement();
        HandleRotation();
    }

    /// <summary>
    /// Handles the jumping functionality.
    /// </summary>
    public void HandleJumping()
    {
        if (_isGrounded)
        {
            _animatorManager.Animator.SetBool("isJumping", true);
            _animatorManager.PlayTargetAnimation("Jump", false);

            float jumpVelocity = Mathf.Sqrt(-2 * _gravityIntensity * _jumpHeight);
            Vector3 playerVelocity = _moveDirection;

            playerVelocity.y = jumpVelocity;
            _playerRigidbody.velocity = playerVelocity;

            _isJumping = false;
        }
    }

    /// <summary>
    /// Sets the isJumping flag.
    /// </summary>
    /// <param name="isJumping"></param>
    public void SetIsJumping(bool isJumping)
    {
        _isJumping = isJumping;
    }

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerManager = GetComponent<PlayerManager>();
        _animatorManager = GetComponent<AnimatorManager>();
    }

    private void HandleMovement()
    {
        if (_isJumping)
            return;

        _moveDirection = _cameraObject.forward * _inputManager.VerticalInput;
        _moveDirection += _cameraObject.right * _inputManager.HorizontalInput;
        _moveDirection.Normalize();
        _moveDirection.y = 0;

        if (_isSprinting)
        {
            _moveDirection *= _sprintingSpeed;
        }
        else
        {
            if (_inputManager.MoveAmount >= 0.5f)
            {
                _moveDirection *= _runningSpeed;
                _isMoving = true;
            }
            else if (_inputManager.MoveAmount < 0.5f)
            {
                _moveDirection *= _walkingSpeed;
                _isMoving = false;
            }
        }

        Vector3 movementVelocity = _moveDirection;
        _playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if (_isJumping)
            return;

        Vector3 targetDirection = Vector3.zero;
        targetDirection = _cameraObject.forward * _inputManager.VerticalInput;
        targetDirection += _cameraObject.right * _inputManager.HorizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    private void HandleFallingAndLanding()
    {
        Vector3 rayCastOrigin = transform.position;
        Vector3 targetPosition;

        rayCastOrigin.y += _rayCastHeightOffset;
        targetPosition = transform.position;

        if (!_isGrounded && !_isJumping)
        {
            if (!_playerManager.IsInteracting)
            {
                _animatorManager.PlayTargetAnimation("Falling", true);
            }

            _inAirTimer += Time.deltaTime;
            _playerRigidbody.AddForce(transform.forward * _leapingVelocity);
            _playerRigidbody.AddForce(_fallingVelocity * _inAirTimer * -Vector3.up);
        }

        if (Physics.SphereCast(rayCastOrigin, 0.3f, -Vector3.up, out RaycastHit hit, _groundLayer))
        {
            if (!_isGrounded && !_playerManager.IsInteracting)
            {
                _animatorManager.PlayTargetAnimation("Landing", true);
            }

            Vector3 rayCastHitPoint = hit.point;
            targetPosition.y = rayCastHitPoint.y;
            _inAirTimer = 0;
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        if (_isGrounded && !_isJumping)
        {
            if (_playerManager.IsInteracting || _inputManager.MoveAmount > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
    }
}
