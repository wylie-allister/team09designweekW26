using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public static CharacterSelectManager Instance;
    // This list stores multiple player scripts within itself
    private List<PlayerCharacterSelector> players = new List<PlayerCharacterSelector>();
    // Checks if countdown is running
    private bool countdownRunning = false;

    private void Awake()
    {
        // instance refers to a unique copy of a script 
        Instance = this;
    }
    // This method is called from "PlayerCharacterSelector" script
    public void RegisterPlayer(PlayerCharacterSelector player)
    {
        // Checks if that player isn't already added before adding them (No duplicates)
        if (!players.Contains(player))
        {
            players.Add(player);
            Debug.Log(players.Count);
        }
    }
    // This method is called from "PlayerCharacterSelector" script
    public void UnregisterPlayer(PlayerCharacterSelector player)
    {
        // Removes a player from the list 
        players.Remove(player);
    }
    // This method is called from "PlayerCharacterSelector" script
    public void UpdateCounts()
    {
        // Keeps track of the amount of one character
        int bunnyCount = 0;
        int foxCount = 0;
        // Checks every value in player list
        foreach (var player in players)
        {
            // Sets count to how many bunnies are active
            if (player.CurrentType == PlayerCharacterSelector.CharacterType.Bunny)
            {
                bunnyCount++;
            }
            // Sets count how many foxes are active
            if (player.CurrentType == PlayerCharacterSelector.CharacterType.Fox)
            {
                foxCount++;
            }
        }
        // If there are 2 bunnies and 2 foxes
        if (bunnyCount >= 2 && foxCount >= 2)
        {
            // If countdown already true don't call method again
            if (!countdownRunning)
            {
                StartCoroutine(StartCountdown());
            }
        }
        else
        {
            // When there aren't 2 bunnies and 2 foxes stop the countdown and cancel the countdown method
            countdownRunning = false;
            StopAllCoroutines();
        }
    }

    private IEnumerator StartCountdown()
    {
        // Bool check stops method from being called twice
        countdownRunning = true;
        // Add some Ui elements here 
        Debug.Log("Teams ready! Starting in 3 Seconds");
        yield return new WaitForSeconds(3f);
        // Put the logic of whatever we want the switch to gameplay to be
        Debug.Log("Begin Game");
    }
}
