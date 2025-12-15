using UnityEngine;

[RequireComponent(typeof(AnimatorManager))]
public class InputManager : MonoBehaviour
{
    public float verticalInput;
    public float horizontalInput;

    public float cameraInputX;
    public float cameraInputY;

    private Vector2 _movementInput;
    private Vector2 _cameraInput;

    private float _moveAmount;
    private PlayerControls _playerControls;
    private AnimatorManager _animatorManager;

    public void HandleAllInputs()
    {
        HandleMovementInput();
    }

    public void HandleMovementInput()
    {
        verticalInput = _movementInput.y;
        horizontalInput = _movementInput.x;

        cameraInputX = _cameraInput.x;
        cameraInputY = _cameraInput.y;

        _moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        _animatorManager.UpdateAnimatorValues(0, _moveAmount);
    }

    private void Awake()
    {
        _animatorManager = GetComponent<AnimatorManager>();
    }

    private void OnEnable()
    {
        if (_playerControls == null)
        {
            _playerControls = new PlayerControls();
            _playerControls.PlayerMovement.Movement.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
            _playerControls.PlayerMovement.CameraMovement.performed += ctx => _cameraInput = ctx.ReadValue<Vector2>();
        }

        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }
}
