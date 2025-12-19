using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Scripts References")]
    [SerializeField] private InputManager _inputManager;

    [Header("Movement Flags")]
    [SerializeField] private bool _isMoving;
    [SerializeField] private bool _isSprinting;

    [Header("Movement")]
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private float _runningSpeed = 5f;
    [SerializeField] private float _walkingSpeed = 1.5f;
    [SerializeField] private float _sprintingSpeed = 7f;
    [SerializeField] private float _rotationSpeed = 12f;

    // private Transform _cameraObject;
    public Transform _cameraObject;
    private Rigidbody _playerRigidbody;

    public bool IsSprinting { get => _isSprinting; set => _isSprinting = value; }

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerRigidbody = GetComponent<Rigidbody>();
        // _cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
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
}
