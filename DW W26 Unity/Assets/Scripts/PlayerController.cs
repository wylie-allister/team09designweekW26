using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Which player number this is
    [field: SerializeField] public int PlayerNumber { get; private set; }

    // The color assigned to this player
    [field: SerializeField] public Color PlayerColor { get; private set; }

    // Used to change sprite color
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }

    // Used for physics movement
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }


    [Header("Top-Down Movement")]

    // Normal walking speed
    [field: SerializeField] public float MoveSpeed { get; private set; } = 6f;

    // Reference to the players input component
    private PlayerInput playerInput;

    // The "Move" action from the input system
    private InputAction inputActionMove;

    // Current movement input from joystick
    private Vector2 moveInput;

    // Stires the last direction the player moved (dash distraction)
    private Vector2 lastMoveDir = Vector2.right;

    // Reference to PlayerRole (Rabbit or Fox)
    private PlayerRole playerRole;

    [Header("Rabbit Dash")]

    // How fast the rabbit dashes
    [SerializeField] private float rabbitDashSpeed = 25f;

    // How long the dash lasts
    [SerializeField] private float rabbitDashDuration = 0.3f;

    // How long before it can be used again
    [SerializeField] private float rabbitDashCooldown = 5.0f;

    // When the player is allowed to dash again
    private float nextDashTime = 0f;

    // Is the player currently dashing?
    private bool isDashing;

    // When the dash should stop
    private float dashEndTime;

    // Direction of the dash
    private Vector2 dashDirection;

    // Speed of the dash
    private float dashSpeed;

    private void Awake()
    {
        // Get the playable component (Rabbit or Fox)
        playerRole = GetComponent<PlayerRole>();
    }

    // This starts a dash
    public void StartDash(Vector2 direction, float speed, float duration)
    {
        // Store dash direction    
        dashDirection = direction.normalized;

        // Store dash speed
        dashSpeed = speed;

        // Mark player as currently dashing
        isDashing = true;

        // Set time when dash should end
        dashEndTime = Time.time + duration;
    }

    // Called when player is assigned a color on spawn (Raphs stuff)
    public void AssignColor(Color color)
    {
        PlayerColor = color;

        // Actually change the sprite color
        if (SpriteRenderer != null)
        {
            SpriteRenderer.color = color;
        } 
    }

    // Called by PlayerSpawn when player joins
    public void AssignPlayerInputDevice(PlayerInput playerInput)
    {
        this.playerInput = playerInput;

        // Find the Move action inside the Player action map
        inputActionMove = playerInput.actions.FindAction("Player/Move");
    }

    // Assign the player number
    public void AssignPlayerNumber(int playerNumber)
    {
        PlayerNumber = playerNumber;
    }

    private void Update()
    {
        // Read joystick movement every frame
        if (inputActionMove != null)
        {
            moveInput = inputActionMove.ReadValue<Vector2>();

            // If player is actually moving then update the last direction
            if (moveInput.sqrMagnitude > 0.01f)
            {
                lastMoveDir = moveInput.normalized;
            }
        }
    }

    private void FixedUpdate()
    {
        // If currently dashing    
        if (isDashing)
        {
            // If dash time is over
            if (Time.time >= dashEndTime)
            {
                isDashing = false;
            }
            else
            {
                // Force dash movement
                Rigidbody2D.linearVelocity = dashDirection * dashSpeed;

                return; // Skip normal movement
            }
        }

        // Normal walking movement
        Vector2 desired = moveInput;

        // Stop diagonal from being faster
        if (desired.sqrMagnitude > 1f)
        {
            desired = desired.normalized;
        }
           
        // Apply walking velocity
        Rigidbody2D.linearVelocity = desired * MoveSpeed;
    }

    // The Dash input action
    public void OnDash()
    {
        // Only rabbits can dash
        if (playerRole != null && playerRole.role != PlayerRole.Role.Rabbit)
        {
            return;
        }

        // If still on cooldown do nothing
        if (Time.time < nextDashTime)
        {
            return;
        }
           
        // Set next allowed dash time
        nextDashTime = Time.time + rabbitDashCooldown;

        // Start the dash in the last movement direction
        StartDash(lastMoveDir, rabbitDashSpeed, rabbitDashDuration);
    }

    // Autofill missing components in inspector
    private void OnValidate() => Reset();

    private void Reset()
    {
        if (Rigidbody2D == null)
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        if (SpriteRenderer == null)
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
