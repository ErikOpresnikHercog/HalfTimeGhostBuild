using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Press E while close to a soul or pedestal to interact.
/// Add this component to the same GameObject as the Player component.
/// </summary>
[RequireComponent(typeof(Player))]
public class PlayerInteraction : MonoBehaviour
{
    [Min(0.1f)]
    public float interactionRadius = 1.4f;

    [Tooltip("Layers that contain soul and pedestal colliders.")]
    public LayerMask interactableLayers = ~0;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void Interact()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            interactionRadius,
            interactableLayers
        );

        SoulPickup closestSoul = null;
        SoulPedestal closestPedestal = null;
        float closestSoulDistance = float.MaxValue;
        float closestPedestalDistance = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            SoulPickup soul = hit.GetComponentInParent<SoulPickup>();
            if (soul != null && soul.isActiveAndEnabled)
            {
                float distance = Vector2.Distance(transform.position, soul.transform.position);
                if (distance < closestSoulDistance)
                {
                    closestSoulDistance = distance;
                    closestSoul = soul;
                }
            }

            SoulPedestal pedestal = hit.GetComponentInParent<SoulPedestal>();
            if (pedestal != null && pedestal.isActiveAndEnabled)
            {
                float distance = Vector2.Distance(transform.position, pedestal.transform.position);
                if (distance < closestPedestalDistance)
                {
                    closestPedestalDistance = distance;
                    closestPedestal = pedestal;
                }
            }
        }

        // When carrying souls, try the pedestal first. Otherwise, try a soul first.
        if (player.HasSouls)
        {
            if (closestPedestal != null && closestPedestal.TryPlaceSoul(player))
                return;

            if (closestSoul != null)
                closestSoul.TryPickup(player);
        }
        else
        {
            if (closestSoul != null && closestSoul.TryPickup(player))
                return;

            if (closestPedestal != null)
                closestPedestal.TryPlaceSoul(player);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}
