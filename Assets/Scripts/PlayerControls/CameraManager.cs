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

    [Header("Mouse Rotation LImits")]
    [SerializeField] private bool _enableMouseLocking = true;
    [SerializeField] private float _maximumLookAngle = 360f;
    [SerializeField] private float _minimumLookAngle = -360f;

    public void HandleAllCameraMovement()
    {
        HandleCursorLock();
        FollowTarget();
        RotateCamera();
    }

    private void Awake()
    {
        _inputManager = FindObjectOfType<InputManager>();
        _playerTransform = FindObjectOfType<PlayerManager>().transform;

        // Lock cursor on start if enabled
        if (_enableMouseLocking)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void HandleCursorLock()
    {
        if (!_enableMouseLocking) return;

        // Toggle cursor lock with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            // Re-lock cursor when clicking in game window
            if (Cursor.lockState != CursorLockMode.Locked && Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
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
        float inputMagnitude = Mathf.Abs(_inputManager.CameraInputX) + Mathf.Abs(_inputManager.CameraInputY);
        float sensitivityMultiplier = inputMagnitude > 1f ? _mouseSensitivityMultiplier : _gamepadSensitivityMultiplier;

        // Make rotation frame-rate independent by multiplying with Time.deltatime
        _lookAngle += _inputManager.CameraInputX * _cameraLookSpeed * sensitivityMultiplier * Time.deltaTime;
        _pivotAngle -= _inputManager.CameraInputY * _cameraPivotSpeed * sensitivityMultiplier * Time.deltaTime;

        // Clamp the look angle to prevent full 360 rotation if desired
        _lookAngle = Mathf.Clamp(_lookAngle, _minimumLookAngle, _maximumLookAngle);

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
