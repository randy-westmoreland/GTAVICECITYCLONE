using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Scripts References")]
    [SerializeField] private InputManager _inputManager;

    [Header("Movement")]
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 12f;

    // private Transform _cameraObject;
    public Transform _cameraObject;
    private Rigidbody _playerRigidbody;

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
        _moveDirection = _cameraObject.forward * _inputManager.verticalInput;
        _moveDirection += _cameraObject.right * _inputManager.horizontalInput;
        _moveDirection.Normalize();
        _moveDirection.y = 0;
        _moveDirection *= _movementSpeed;

        Vector3 movementVelocity = _moveDirection;
        _playerRigidbody.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = _cameraObject.forward * _inputManager.verticalInput;
        targetDirection += _cameraObject.right * _inputManager.horizontalInput;
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
