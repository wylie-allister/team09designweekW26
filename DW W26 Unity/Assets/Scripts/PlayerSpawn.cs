using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawn : MonoBehaviour
{
    // There are 4 spawn positions assigned
    [field: SerializeField] public Transform[] SpawnPoints { get; private set; }

    // Colors that get assigned in order when players join (idk this is raphs stuff)
    [field: SerializeField] public Color[] PlayerColors { get; private set; }

    // Keeps track of how many players joined
    public int PlayerCount { get; private set; }

    ///////////////////////////////////////////////
    
    // These two integers control the bunny player spawn at index 0,1 and fox player spawn at index 2 and 3
    private int nextBunnySpawn = 0;
    private int nextFoxSpawn = 2;
    ///////////////////////////////////////////////

    // This function is automatically called by PlayerInputManager whenever a new player joins the game
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // This will prevent more players then we support in the game     
        int maxPlayerCount = Mathf.Min(SpawnPoints.Length, PlayerColors.Length);

        // If too many players try to join then destroy the extra one
        if (PlayerCount >= maxPlayerCount)
        {
            Destroy(playerInput.gameObject);
            return;
        }

        ///////////////////////////////////////////////
        
        // Figure out which team this player picked

        // Default to bunny incase something goes wrong 
        GameSettings.Team team = GameSettings.Team.Bunny;

        // If the game settings exist (which is does just safety check) try and get the saved team choice
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.teamByPlayerIndex.TryGetValue(playerInput.playerIndex, out team);
        }

        // Based on the team (Bunny or fox) choose the correct spawn index
        int spawnIndex = GetSpawnIndexForTeam(team);

        ///////////////////////////////////////////////

        // Actually move the player to the correct spawn point
        playerInput.transform.position = SpawnPoints[spawnIndex].position;
        playerInput.transform.rotation = SpawnPoints[spawnIndex].rotation;

        ///////////////////////////////////////////////

        // Set their PlayerRole so the gameplay rules work properly
        PlayerRole role = playerInput.GetComponent<PlayerRole>();
        if (role != null)
        {
            role.role = (team == GameSettings.Team.Bunny) ? PlayerRole.Role.Rabbit: PlayerRole.Role.Fox;
        }
        ///////////////////////////////////////////////

        // Assign color based on join order (unimportant this is raphs stuff)
        Color color = PlayerColors[PlayerCount];

        // Increase the player count before assigning number so the PlayerNumber starts at 1 instead of 0
        PlayerCount++;

        // Set up the player controller
        PlayerController playerController = playerInput.gameObject.GetComponent<PlayerController>();
        if (playerController == null)
        {
            return;
        }

        // Tell controller which device it belongs to
        playerController.AssignPlayerInputDevice(playerInput);

        // Assign player number (1,2,3,4)
        playerController.AssignPlayerNumber(PlayerCount);

        // Apply their color
        playerController.AssignColor(color);
    }

    ///////////////////////////////////////////////
    private int GetSpawnIndexForTeam(GameSettings.Team team)
    {
        // If bunny, then first bunny goes to index 0 and second bunny goes to index 1
        if (team == GameSettings.Team.Bunny)
        {
            int index = nextBunnySpawn;
            nextBunnySpawn = (nextBunnySpawn == 0) ? 1 : 0;
            return index;
        }
        // If fox, then first fox goes to index 2 and second fox goes to index 3
        else
        {
            int index = nextFoxSpawn;

            // Toggle between 2 and 3
            nextFoxSpawn = (nextFoxSpawn == 2) ? 3 : 2; 

            return index;
        }
    }
    ///////////////////////////////////////////////

    // This runs if a player leaves which is not gonna happen in our game  (this is raphs stuff)
    public void OnPlayerLeft(PlayerInput playerInput)
    {
       
    }
}
