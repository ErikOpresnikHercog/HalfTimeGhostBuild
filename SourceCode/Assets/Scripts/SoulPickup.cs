using UnityEngine;

/// <summary>
/// Put this on each soul GameObject. The player must press E within
/// PlayerInteraction's radius to collect it.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SoulPickup : MonoBehaviour
{
    [Tooltip("Optional sound played when this soul is collected.")]
    public AudioClip pickupSound;

    private bool collected;

    public bool TryPickup(Player player)
    {
        if (collected || player == null)
            return false;

        // Extra distance check so the soul cannot be picked up remotely.
        PlayerInteraction interaction = player.GetComponent<PlayerInteraction>();
        float allowedDistance = interaction != null ? interaction.interactionRadius : 1.4f;
        if (Vector2.Distance(player.transform.position, transform.position) > allowedDistance)
            return false;

        collected = true;
        player.AddSoul();

        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
        return true;
    }
}
