using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeamSelectUI : MonoBehaviour
{
    // UI panel the players cursor is allowed to move inside of
    public RectTransform moveBounds;

    // Text slots at top left of screen
    public TMP_Text[] statusSlots; 

    // This is called by PlayerInputManager when a new controller joins the scene
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // Get the players index
        int index = playerInput.playerIndex;

        // Get the team selector script connected to that player
        TeamSelector selector = playerInput.GetComponent<TeamSelector>();

        // Give this player the panel bounds so their cursor knows where it is allowed to move
        selector.AssignSlots(moveBounds);

        // Now give the correct status text slot
        if (statusSlots != null && index >= 0 && index < statusSlots.Length)
        {
            // Give this player their text line
            selector.statusText = statusSlots[index];

            // Immediatly update
            selector.ForceRefreshStatusText(); 
        }    
    }
}
