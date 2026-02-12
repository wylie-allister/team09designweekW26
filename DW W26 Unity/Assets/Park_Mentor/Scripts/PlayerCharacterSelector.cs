using UnityEngine;

public class PlayerCharacterSelector : MonoBehaviour
{
    // Stores values (In this case different characters)
    public enum CharacterType
    {
        Shell, Bunny, Fox
    }
    // Prefabs to be spawned in
    [Header("Prefabs")]
    public GameObject shellPrefab;
    public GameObject bunnyPrefab;
    public GameObject foxPrefab;
    private GameObject currentCharacter;
    // Other scripts can access this value but only this script can change it
    public CharacterType CurrentType { get; private set; }

    private void OnEnable()
    {
        // Registers the players to a list in "CharacterSelectManager" script to check for 4 players
        CharacterSelectManager.Instance.RegisterPlayer(this);
    }

    private void OnDisable()
    {
        // Removes a player if they quit the game or gameobject is destroyed
        CharacterSelectManager.Instance.UnregisterPlayer(this);
    }

    void Awake()
    {
        // Players start as the "Shell"
        CurrentType = (CharacterType)(-1);
        SetCharacter(CharacterType.Shell);
    }

    public void SetCharacter(CharacterType type)
    {
        // Makes sure the player doesn't transform into the same character
        if (CurrentType == type)
        {
            return;
        }
        // Deletes the previous character to make room for the new one
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }
        // Instantiate the new character at the same position
        switch (type)
        {
            case CharacterType.Shell:
                {
                    currentCharacter = Instantiate(shellPrefab, gameObject.transform);
                    break;
                }
            case CharacterType.Bunny:
                {
                    currentCharacter = Instantiate(bunnyPrefab, gameObject.transform);
                    break;
                }
            case CharacterType.Fox:
                {
                    currentCharacter = Instantiate(foxPrefab, gameObject.transform);
                    break;
                }
        }
        // Current Type is equal to the new character
        CurrentType = type;
        // Update the character count for the "CharactersSelectManager" Script 
        CharacterSelectManager.Instance.UpdateCounts();
    }
}
