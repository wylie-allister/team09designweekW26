using UnityEngine;

public class BurrowTravelState : MonoBehaviour
{
    // This stores which burrow the player used last, starting at -1 which means none yet
    public int lastPortalId = -1;

    // This stores the exact time in seconds the burrow was last used
    private float lastUseTime = -999f;

    // This checks if enough time has passed to be able to use burrow again
    public bool CanUseBurrow(float cooldown)
    {
        // If the current time is greater than the last time + the cooldown then burrowing is allowed
        bool allowed = Time.time >= lastUseTime + cooldown;

        // If its not allowed just print a debug log (for now)
        if (!allowed)
        {
            Debug.Log($"Cooldown remaining: {(lastUseTime + cooldown) - Time.time:F2}s");
        }

        return allowed;
    }

    // This is called after a burrow is successfully used and updates which burrow was used + what time it was used at
    public void RecordUse(int portalId)
    {
        lastPortalId = portalId;
        lastUseTime = Time.time;
    }
}
