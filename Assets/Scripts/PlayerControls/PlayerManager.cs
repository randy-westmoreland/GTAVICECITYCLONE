using UnityEngine;

[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerManager : MonoBehaviour
{
    private InputManager _inputManager;
    private CameraManager _cameraManager;
    private PlayerMovement _playerMovement;

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerMovement = GetComponent<PlayerMovement>();
        _cameraManager = FindObjectOfType<CameraManager>();
    }

    private void Update()
    {
        _inputManager.HandleAllInputs();
        _cameraManager.HandleAllCameraMovement();
    }

    private void FixedUpdate()
    {
        _playerMovement.HandleAllMovement();
    }
}
