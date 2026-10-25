using UnityEngine;

/// <summary>
/// Opens the outer door when called by SoulPedestal.
/// Assign the blocking collider and, optionally, an Animator or door visual.
/// </summary>
public class SoulDoor : MonoBehaviour
{
    public Collider2D blockingCollider;
    public Animator doorAnimator;

    [Tooltip("Optional visual object to hide when no door animation is assigned.")]
    public GameObject doorVisual;

    [Tooltip("Animator trigger parameter used for the opening animation.")]
    public string openTriggerName = "Open";

    public bool IsOpen { get; private set; }

    public void OpenDoor()
    {
        if (IsOpen)
            return;

        IsOpen = true;

        // The player can now pass through the doorway.
        if (blockingCollider != null)
            blockingCollider.enabled = false;

        if (doorAnimator != null && !string.IsNullOrEmpty(openTriggerName))
        {
            doorAnimator.SetTrigger(openTriggerName);
        }
        else if (doorVisual != null)
        {
            doorVisual.SetActive(false);
        }
        else
        {
            // If the script is on the door itself, hide it as a final fallback.
            gameObject.SetActive(false);
        }

        Debug.Log("Outer door opened: four souls have been placed on the pedestal.", this);
    }
}
