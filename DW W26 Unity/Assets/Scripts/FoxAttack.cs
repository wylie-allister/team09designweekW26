using UnityEngine;

public class FoxAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackRadius = 0.65f;
    [SerializeField] private float attackCooldown = 1.0f;

    [Header("Target Filtering")]
    [SerializeField] private LayerMask playerLayer = ~0; // default = Everything

    [Header("Stun + Drop")]
    [SerializeField] private float stunSeconds = 1.0f;
    [SerializeField] private GameObject carrotDropPrefab;
    [SerializeField] private float dropScatterRadius = 0.35f;

    private float nextAttackTime;
    private PlayerRole myRole;

    private void Awake()
    {
        myRole = GetComponent<PlayerRole>();
    }

    // Called by Input System (Send Messages) from the "Attack" action
    public void OnAttack()
    {
        if (Time.time < nextAttackTime) return;

        // Only fox can attack
        if (myRole == null || myRole.role != PlayerRole.Role.Fox) return;

        if (GameManager.Instance == null || GameManager.Instance.gameEnded) return;

        // Find all colliders in radius
        var hits = Physics2D.OverlapCircleAll(transform.position, attackRadius, playerLayer);

        GameObject bestRabbit = null;
        float bestDist = float.MaxValue;

        foreach (var h in hits)
        {
            if (h == null) continue;
            if (!h.CompareTag("Player")) continue;
            if (h.gameObject == gameObject) continue;

            var targetRole = h.GetComponent<PlayerRole>();
            if (targetRole == null || targetRole.role != PlayerRole.Role.Rabbit) continue;

            var lives = h.GetComponent<PlayerLives>();
            if (lives != null && lives.IsDead) continue;

            float d = Vector2.Distance(transform.position, h.transform.position);
            if (d < bestDist)
            {
                bestDist = d;
                bestRabbit = h.gameObject;
            }
        }

        // No bunny nearby
        if (bestRabbit == null) return;

        // Apply hit
        var targetLives = bestRabbit.GetComponent<PlayerLives>();
        if (targetLives == null) targetLives = bestRabbit.AddComponent<PlayerLives>();
        if (targetLives.IsDead) return;

        targetLives.LoseLife(1);

        // Drop carried carrots as physical pickups
        var inv = bestRabbit.GetComponent<PlayerInventory>();
        if (inv != null)
        {
            int carried = inv.DepositAll();
            if (carried > 0 && carrotDropPrefab != null)
            {
                for (int i = 0; i < carried; i++)
                {
                    Vector2 offset = Random.insideUnitCircle * dropScatterRadius;
                    Vector3 pos = bestRabbit.transform.position + (Vector3)offset;
                    Instantiate(carrotDropPrefab, pos, Quaternion.identity);
                }
            }
        }

        // Stun + respawn
        GameManager.Instance.StunThenRespawnRabbit(bestRabbit, stunSeconds);

        // Check fox win
        GameManager.Instance.CheckFoxWin();

        // Cooldown
        nextAttackTime = Time.time + attackCooldown;

        Debug.Log("Fox attack used!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
