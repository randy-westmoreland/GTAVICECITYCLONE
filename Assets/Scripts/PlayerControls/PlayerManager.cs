using UnityEngine;

/// <summary>
/// Manages the player character functionalities.
/// </summary>
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(Animator))]
public class PlayerManager : MonoBehaviour
{
    // References
    private InputManager _inputManager;
    private CameraManager _cameraManager;
    private PlayerMovement _playerMovement;
    private Animator _animator;

    [SerializeField] private bool _isInteracting;

    /// <summary>
    /// Gets or sets a value indicating whether the player is interacting.
    /// </summary>
    public bool IsInteracting { get => _isInteracting; set => _isInteracting = value; }

    private void Awake()
    {
        _inputManager = GetComponent<InputManager>();
        _playerMovement = GetComponent<PlayerMovement>();
        _animator = GetComponent<Animator>();
        _cameraManager = FindAnyObjectByType<CameraManager>();
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

    private void LateUpdate()
    {
        _isInteracting = _animator.GetBool("isInteracting");
        _playerMovement.IsJumping = _animator.GetBool("isJumping");
        _animator.SetBool("isGrounded", _playerMovement.IsGrounded);
    }
}
