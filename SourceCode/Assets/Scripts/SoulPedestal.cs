
using UnityEngine;
using TMPro;

/// <summary>
/// Place this on the pedestal. Press E nearby to deposit one carried soul.
/// Each deposited soul appears at the next placement point.
/// The door opens when enough souls have been deposited.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class SoulPedestal : MonoBehaviour
{
    [Header("Soul Settings")]
    [Min(1)]
    public int soulsRequired = 4;

    [SerializeField]
    private int soulsPlaced = 0;

    [Tooltip("Prefab instantiated when a soul is placed.")]
    public GameObject soulPrefab;

    [Tooltip("The four positions where placed souls appear.")]
    public Transform[] soulLocations = new Transform[4];

    [Header("Door")]
    public SoulDoor outerDoor;

    [Header("UI")]
    public TMP_Text progressText;

    private bool doorOpened;

    public int SoulsPlaced => soulsPlaced;
    public bool IsComplete => soulsPlaced >= soulsRequired;

    private void Start()
    {
        // Ensure the counter is within the valid range.
        soulsPlaced = Mathf.Clamp(soulsPlaced, 0, soulsRequired);

        UpdateProgressText();

        // Re-create visuals for souls already counted in the Inspector.
        for (int i = 0; i < soulsPlaced; i++)
        {
            SpawnSoulAtLocation(i);
        }

        if (IsComplete)
            OpenDoor();
    }

    public bool TryPlaceSoul(Player player)
    {
        if (player == null || !player.HasSouls || IsComplete)
            return false;

        // Do not consume a soul if the visual setup is missing.
        if (soulPrefab == null)
        {
            Debug.LogWarning("SoulPedestal: Assign a Soul Prefab.", this);
            return false;
        }

        if (soulLocations == null ||
            soulsPlaced >= soulLocations.Length ||
            soulLocations[soulsPlaced] == null)
        {
            Debug.LogWarning(
                "SoulPedestal: Assign a location for the next soul.",
                this
            );
            return false;
        }

        if (!player.UseSoul())
            return false;

        SpawnSoulAtLocation(soulsPlaced);

        soulsPlaced++;
        UpdateProgressText();

        if (IsComplete)
            OpenDoor();

        return true;
    }

    private void SpawnSoulAtLocation(int index)
    {
        if (soulPrefab == null ||
            soulLocations == null ||
            index < 0 ||
            index >= soulLocations.Length ||
            soulLocations[index] == null)
        {
            return;
        }

        Transform point = soulLocations[index];

        Instantiate(
            soulPrefab,
            point.position,
            point.rotation,
            point
        );
    }

    private void OpenDoor()
    {
        if (doorOpened)
            return;

        doorOpened = true;

        if (outerDoor != null)
        {
            outerDoor.OpenDoor();
        }
        else
        {
            Debug.LogWarning(
                "SoulPedestal: Assign the Outer Door in the Inspector.",
                this
            );
        }
    }

    private void UpdateProgressText()
    {
        if (progressText != null)
        {
            progressText.text =
                $"{soulsPlaced}/{soulsRequired}";
        }
    }
}
