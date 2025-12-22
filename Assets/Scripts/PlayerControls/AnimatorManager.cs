using UnityEngine;

/// <summary>
/// Manages the animator component for the player character.
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimatorManager : MonoBehaviour
{
    private Animator _animator;
    private int _horizontal;
    private int _vertical;

    /// <summary>
    /// Gets or sets the animator component.
    /// </summary>
    public Animator Animator { get => _animator; set => _animator = value; }

    /// <summary>
    /// Updates the animator parameters based on movement input.
    /// </summary>
    /// <param name="horizontalMovement"></param>
    /// <param name="verticalMovement"></param>
    /// <param name="isSprinting"></param>
    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontal;
        float snappedVertical;

        #region Snapped Horizontal
        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            snappedHorizontal = 0.5f;
        }
        else if (horizontalMovement >= 0.55f)
        {
            snappedHorizontal = 1f;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            snappedHorizontal = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            snappedHorizontal = -1f;
        }
        else
        {
            snappedHorizontal = 0f;
        }
        #endregion

        #region Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement >= 0.55f)
        {
            snappedVertical = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement < -0.55f)
        {
            snappedVertical = -1f;
        }
        else
        {
            snappedVertical = 0f;
        }
        #endregion

        if (isSprinting)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2f;
        }

        Animator.SetFloat(_horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
        Animator.SetFloat(_vertical, snappedVertical, 0.1f, Time.deltaTime);
    }

    /// <summary>
    /// Plays the target animation with the specified interaction state.
    /// </summary>
    /// <param name="targetAnimation"></param>
    /// <param name="isInteracting"></param>
    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        Animator.SetBool("isInteracting", isInteracting);
        Animator.CrossFade(targetAnimation, 0.2f);
    }

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        _horizontal = Animator.StringToHash("Horizontal");
        _vertical = Animator.StringToHash("Vertical");
    }
}
