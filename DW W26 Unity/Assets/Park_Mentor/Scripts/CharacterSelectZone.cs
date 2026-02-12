using UnityEngine;

public class CharacterSelectZone : MonoBehaviour
{
    // Public varaible of the enum in "PlayerCharacterSelector" script, This determines what character the player will turn into
    public PlayerCharacterSelector.CharacterType zoneType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When any collider enters, Set that gameobject's "PlayerCharacterSelector" script to a variable 
        PlayerCharacterSelector selector = other.GetComponentInParent<PlayerCharacterSelector>();

        if (selector != null)
        {
            // Calls public method of "PlayerCharacterSelector" to change into that charater
            selector.SetCharacter(zoneType);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // When any collider exits, Set that gameobject's "PlayerCharacterSelector" script to a variable 
        PlayerCharacterSelector selector = other.GetComponentInParent<PlayerCharacterSelector>();

        if (selector != null)
        {
            // Calls public method of "PlayerCharacterSelector" to change back into the player shell
            selector.SetCharacter(PlayerCharacterSelector.CharacterType.Shell);
        }
    }
}
