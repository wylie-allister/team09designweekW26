using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TeamSelector : MonoBehaviour
{
    // SETTINGS FOR THE CURSOR
    [Header("Cursor Setup (UI)")]
    
    // This is the UI cursor prefab that moves around the screen
    [SerializeField] private RectTransform selectorCursorPrefab;

    // This is the UI panel the cursor is allowed to move inside
    [SerializeField] private RectTransform moveBounds;

    // How many pixels per second the cursor is allowed to move
    [SerializeField] private float cursorSpeed = 800f;

    // STATUS TEXT
    [Header("Status")]
    
    // This is the text that shows the P1: Bunny or P2: Fox on top left of screen
    public TMP_Text statusText;

    // The actual spawned cursor instance
    private RectTransform cursor;

    // True once the player locks in their choice
    private bool lockedIn;

    // Which playuer this is
    private int playerIndex;

    // Stores joystick movement input
    private Vector2 moveInput;

    // Small delay so it doesnt instantly trigger on spawn
    private float ignoreSubmitUntilTime;

    // This is called by TeamSelectUI and gives this player the panel bounds it can move inside of
    public void AssignSlots(RectTransform bounds)
    {
        moveBounds = bounds;
    }

    // Runs when the player joins
    private void Start()
    {
        // Which player this is
        playerIndex = GetComponent<PlayerInput>().playerIndex;

        // Spawn the cursor inside of the moveBounds panel
        cursor = Instantiate(selectorCursorPrefab, moveBounds);

        // Start it centered
        cursor.anchoredPosition = Vector2.zero;

        // Small delay so pressing A doesnt instantly lock in (hope this works)
        ignoreSubmitUntilTime = Time.time + 0.25f;

        // Update the status text
        UpdateStatusPreview();
    }

    // Movement input called from PlayerInput Move action (made this for the starting screen)
    public void OnMove(InputAction.CallbackContext context)
    {
        // If player already locked in dont allow any movement
        if (lockedIn)
        {
            return;
        }

        // Store the joystick direction
        moveInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        // If player already locked in dont allow any movement
        if (lockedIn)
        {
            return;
        }

        if (cursor == null)
        {
            return;
        }

        // Move cursor based on joystick input
        Vector2 delta = moveInput * cursorSpeed * Time.deltaTime;
        cursor.anchoredPosition += delta;

        // Update text to show what team we are hovering over
        UpdateStatusPreview();
    }

    // Locks in team selection
    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        // Prevent accidental instant submit on spawn
        if (Time.time < ignoreSubmitUntilTime)
        {
            return;
        }

        // If already locked in then ignore
        if (lockedIn)
        {
            return;
        }

        // Mark as located
        lockedIn = true;

        // Figure out which team based on cursor position
        var teamEnum = GetTeamFromCursor();

        // Save this team inside GameSettings so it carries to next scene
        GameSettings.Instance.SetTeam(playerIndex, teamEnum);

        // Log what they picked Debugging only but nice to have
        Debug.Log($"Player {playerIndex + 1} selected {teamEnum}");

        // Update UI text to show final selection
        if (statusText != null)
        {
            statusText.text = $"P{playerIndex + 1}: {teamEnum}";
        }
            
    }

    // Cancel which allows player to unlock and choose again
    public void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        if (!lockedIn)
        {
            return;
        }

        // Unlock selection
        lockedIn = false;

        // Refresh previous text
        UpdateStatusPreview();
    }

    // Start game Options button which loads next scene
    public void OnStartGame(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }

        // Load main game scene
        SceneManager.LoadScene("Dual Monitor Scene");
    }

    private void UpdateStatusPreview()
    {
        if (lockedIn)
        {
            return;
        }

        var team = GetTeamFromCursor();

        if (statusText != null)
        {
            statusText.text = $"P{playerIndex + 1}: {team}";
        }  
    }

    // Decideds team based on cursor X position left side is bunny right side is fox
    private GameSettings.Team GetTeamFromCursor()
    {
        // If cursor hasnt spawned yet just default to bunny instead of tweaking
        if (cursor == null)
        {
            return GameSettings.Team.Bunny;
        }

        return cursor.anchoredPosition.x < 0f ? GameSettings.Team.Bunny : GameSettings.Team.Fox;
    }

    // Makes sure to force the status text to refresh manually
    public void ForceRefreshStatusText()
    {
        UpdateStatusPreview();
    }
}
