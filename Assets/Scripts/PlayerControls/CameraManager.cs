using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private InputManager _inputManager;
    private Vector3 _cameraFollowVelocity = Vector3.zero;

    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _cameraPivot;

    [Header("Camera Movement and Rotation")]
    [SerializeField] private float _cameraFollowSpeed = 0.1f;
    [SerializeField] private float _cameraLookSpeed = 200f;
    [SerializeField] private float _cameraPivotSpeed = 200f;
    [SerializeField] private float _lookAngle;
    [SerializeField] private float _pivotAngle;

    [Header("Input Sensitivity")]
    [SerializeField] private float _mouseSensitivityMultiplier = 0.1f;
    [SerializeField] private float _gamepadSensitivityMultiplier = 1f;

    [Header("Camera Pivot Limits")]
    [SerializeField] private float _minimumPivotAngle = -30f;
    [SerializeField] private float _maximumPivotAngle = 30f;

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
    }

    private void Awake()
    {
        _inputManager = FindObjectOfType<InputManager>();
        _playerTransform = FindObjectOfType<PlayerManager>().transform;
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, _playerTransform.position, ref _cameraFollowVelocity, _cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        Vector3 rotation;
        Quaternion targetRotation;

        // Detect input type based on magnitude and apply appropriate sensitivity
        float inputMagnitude = Mathf.Abs(_inputManager.cameraInputX) + Mathf.Abs(_inputManager.cameraInputY);
        float sensitivityMultiplier = inputMagnitude > 1f ? _mouseSensitivityMultiplier : _gamepadSensitivityMultiplier;

        // Make rotation frame-rate independent by multiplying with Time.deltatime
        _lookAngle += _inputManager.cameraInputX * _cameraLookSpeed * sensitivityMultiplier * Time.deltaTime;
        _pivotAngle -= _inputManager.cameraInputY * _cameraPivotSpeed * sensitivityMultiplier * Time.deltaTime;

        // Clamp the pivot angle to prevent flipping
        _pivotAngle = Mathf.Clamp(_pivotAngle, _minimumPivotAngle, _maximumPivotAngle);

        rotation = Vector3.zero;
        rotation.y = _lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = _pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        _cameraPivot.localRotation = targetRotation;
    }
}
