using UnityEngine;

public class BurrowPortal : MonoBehaviour
{
    // Each burrow will have an identity ID (0,1,2,3) Which helps to track which one was used
    [Header("Burrow Identity")]
    public int portalId = 0;

    // Location where the player will be sent too
    [Header("Where This Burrow Will Send Player")]
    public Transform destination;


    [Header("Rules")]
    // If true only rabbits can use this portal
    public bool rabbitsOnly = true;

    // How many seconds before this player can use any burrow again
    public float globalCooldownSeconds = 5f;

    // Prevents players from going back into the same burrow
    public bool blockSamePortal = true;

    // Runs when something enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If its not tagged "Player" then ignore it
        if (!other.CompareTag("Player"))
        {
            return;
        }

        // Get the PlayerRole component from whoever touched the portal
        PlayerRole role = other.GetComponent<PlayerRole>();
      
        // If portal only allows rabbits and this is not a rabbit then stop
        if (rabbitsOnly && role.role != PlayerRole.Role.Rabbit)
        {
            return;
        }

        // Get or make the BurrowTravelState component on this player
        BurrowTravelState travel = other.GetComponent<BurrowTravelState>();

        // cooldown check
        if (!travel.CanUseBurrow(globalCooldownSeconds))
        {
            return;
        }

        // same-portal check
        if (blockSamePortal && travel.lastPortalId == portalId)
        {
            return;
        }

        if (destination == null)
        {
            return;
        }

        // Teleport
        Transform t = other.transform;
        t.position = destination.position;

        // Stop carry-over movement
        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Record use
        travel.RecordUse(portalId);
    }
}