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

    private bool _sprintInput;
    private float _moveAmount;

    // References
    private PlayerControls _playerControls;
    private PlayerMovement _playerMovement;
    private AnimatorManager _animatorManager;

    // Properties
    public float CameraInputX { get => _cameraInputX; set => _cameraInputX = value; }
    public float CameraInputY { get => _cameraInputY; set => _cameraInputY = value; }
    public float VerticalInput { get => _verticalInput; set => _verticalInput = value; }
    public float HorizontalInput { get => _horizontalInput; set => _horizontalInput = value; }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintInput();
    }

    private void Awake()
    {
        _animatorManager = GetComponent<AnimatorManager>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void HandleMovementInput()
    {
        _verticalInput = _movementInput.y;
        _horizontalInput = _movementInput.x;

        _cameraInputX = _cameraInput.x;
        _cameraInputY = _cameraInput.y;

        _moveAmount = Mathf.Clamp01(Mathf.Abs(_horizontalInput) + Mathf.Abs(_verticalInput));
        _animatorManager.UpdateAnimatorValues(0, _moveAmount);
    }

    private void HandleSprintInput()
    {
        if (_sprintInput && _moveAmount > 0.5f)
        {
            _playerMovement.IsSprinting = true;
        }
        else
        {
            _playerMovement.IsSprinting = false;
        }
    }

    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.PlayerMovement.Movement.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
            _playerControls.PlayerMovement.CameraMovement.performed += ctx => _cameraInput = ctx.ReadValue<Vector2>();
            _playerControls.PlayerActions.B.performed += ctx => _sprintInput = true;
            _playerControls.PlayerActions.B.canceled += ctx => _sprintInput = false;
        }

        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
