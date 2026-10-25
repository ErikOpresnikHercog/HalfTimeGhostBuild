using UnityEngine;

/// <summary>
/// Keeps track of the souls currently carried by the player.
/// Replaces the original Player.cs while preserving GhostsCollected.
/// </summary>
public class Player : MonoBehaviour
{
    [Min(0)]
    public int GhostsCollected = 0;

    public bool HasSouls => GhostsCollected > 0;

    public void AddSoul()
    {
        GhostsCollected++;
    }

    /// <summary>Removes one carried soul. Returns false if the player has none.</summary>
    public bool UseSoul()
    {
        if (GhostsCollected <= 0)
            return false;

        GhostsCollected--;
        return true;
    }
}
