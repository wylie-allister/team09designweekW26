using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    // This makes a global version of the script letting any other script access it
    public static GameSettings Instance { get; private set; }

    // There are 2 possible teams in the game
    public enum Team { Bunny, Fox }

    // This dictionary stores which teams each player has picked the key is the playerIndex from PlayerInput and the value is the team they selected
    public Dictionary<int, Team> teamByPlayerIndex = new Dictionary<int, Team>();

    private void Awake()
    {
        // Set this as a global instance 
        Instance = this;
        
        // This makes sure GameSettings survives when we change scenes
        DontDestroyOnLoad(gameObject);
    }

    // This saves or updates the team choice for a player if it already exists in the dictionary then it just overwrites their intial team
    public void SetTeam(int playerIndex, Team team)
    {
        teamByPlayerIndex[playerIndex] = team;
    }

    // This checks whether a player already picked a team, returns true if they have
    public bool HasTeam(int playerIndex)
    {
        return teamByPlayerIndex.ContainsKey(playerIndex);
    }
}
