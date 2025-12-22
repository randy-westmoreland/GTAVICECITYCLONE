using UnityEngine;

[RequireComponent(typeof(AnimatorManager))]
[RequireComponent(typeof(PlayerMovement))]
public class InputManager : MonoBehaviour
{
    [SerializeField] private float _verticalInput;
    [SerializeField] private float _horizontalInput;

    [SerializeField] private float _cameraInputX;
    [SerializeField] private float _cameraInputY;

    private Vector2 _movementInput;
    private Vector2 _cameraInput;

    private float _moveAmount;

    [Header("Input Button Flag")]
    private bool _sprintInput;
    private bool _jumpInput;

    // References
    private PlayerControls _playerControls;
    private PlayerMovement _playerMovement;
    private AnimatorManager _animatorManager;

    /// <summary>
    /// Gets or sets the camera input on the X axis.
    /// </summary>
    public float CameraInputX { get => _cameraInputX; set => _cameraInputX = value; }

    /// <summary>
    /// Gets or sets the camera input on the Y axis.
    /// </summary>
    public float CameraInputY { get => _cameraInputY; set => _cameraInputY = value; }

    /// <summary>
    /// Gets or sets the vertical movement input.
    /// </summary>
    public float VerticalInput { get => _verticalInput; set => _verticalInput = value; }

    /// <summary>
    /// Gets or sets the horizontal movement input.
    /// </summary>
    public float HorizontalInput { get => _horizontalInput; set => _horizontalInput = value; }

    /// <summary>
    /// Gets or sets the move amount.
    /// </summary>
    public float MoveAmount { get => _moveAmount; set => _moveAmount = value; }

    /// <summary>
    /// Handles all input functionalities.
    /// </summary>
    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintInput();
        HandleJumpingInput();
    }

    private void Awake()
    {
        _animatorManager = GetComponent<AnimatorManager>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.PlayerMovement.Movement.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
            _playerControls.PlayerMovement.CameraMovement.performed += ctx => _cameraInput = ctx.ReadValue<Vector2>();

            // Sprinting
            _playerControls.PlayerActions.B.performed += ctx => _sprintInput = true;
            _playerControls.PlayerActions.B.canceled += ctx => _sprintInput = false;

            // 
            _playerControls.PlayerActions.Jump.performed += ctx => _jumpInput = true;
        }

        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }

    private void HandleMovementInput()
    {
        _verticalInput = _movementInput.y;
        _horizontalInput = _movementInput.x;

        _cameraInputX = _cameraInput.x;
        _cameraInputY = _cameraInput.y;

        MoveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalInput) + Mathf.Abs(_verticalInput));
        _animatorManager.UpdateAnimatorValues(0, MoveAmount, _playerMovement.IsSprinting);
    }

    private void HandleSprintInput()
    {
        if (_sprintInput && MoveAmount > 0.5f)
        {
            _playerMovement.IsSprinting = true;
        }
        else
        {
            _playerMovement.IsSprinting = false;
        }
    }

    private void HandleJumpingInput()
    {
        if (_jumpInput)
        {
            _jumpInput = false;
            _playerMovement.IsJumping = true;
            _playerMovement.HandleJumping();
        }
    }
}
