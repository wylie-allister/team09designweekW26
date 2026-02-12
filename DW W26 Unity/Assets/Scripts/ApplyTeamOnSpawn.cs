using UnityEngine;
using UnityEngine.InputSystem;

public class ApplyTeamOnSpawn : MonoBehaviour
{
    // Reference to the PlayerRole component 
    public PlayerRole playerRole;

    // Reference to the SpriteRenderer so we can change the character art
    public SpriteRenderer spriteRenderer;

    // The sprite used if this player is a Bunny
    public Sprite bunnySprite;

    // The sprite used if this player is a Fox
    public Sprite foxSprite;

    private void Start()
    {
        // Get the PlayerInput component 
        var input = GetComponent<PlayerInput>();

        // Every joining player has a playerIndex
        int index = input.playerIndex;

        // Check GameSettings to see what team this player picked 
        if (!GameSettings.Instance.teamByPlayerIndex.TryGetValue(index, out var team))
        {
            team = GameSettings.Team.Bunny;
        }

        // Set the PlayerRole script so the game knows whether or not this player is a Rabbit or a Fox
        if (playerRole != null)
        {
            playerRole.role = (team == GameSettings.Team.Bunny) ? PlayerRole.Role.Rabbit : PlayerRole.Role.Fox;
        }
          
        // Change the sprite so the character matches what they picked
        if (spriteRenderer != null)
        {
            // If player picked bunny, apply bunny sprite
            if (team == GameSettings.Team.Bunny && bunnySprite != null)
            {
                spriteRenderer.sprite = bunnySprite;
            }

            // If player picked fox, apply fox sprite
            if (team == GameSettings.Team.Fox && foxSprite != null)
            {
                spriteRenderer.sprite = foxSprite;
            }
        }
    }
}
