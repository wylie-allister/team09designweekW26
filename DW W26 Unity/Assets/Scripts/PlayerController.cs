using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public int PlayerNumber { get; private set; }
    [field: SerializeField] public Color PlayerColor { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

    [Header("Top-Down Movement")]
    [field: SerializeField] public float MoveSpeed { get; private set; } = 6f;

    // Player input information
    private PlayerInput playerInput;
    private InputAction inputActionMove;

    private Vector2 moveInput;

    // Assign color value on spawn from main spawner
    public void AssignColor(Color color)
    {
        PlayerColor = color;

        if (SpriteRenderer == null)
            Debug.Log($"Failed to set color to {name} {nameof(PlayerController)}.");
        else
            SpriteRenderer.color = color;
    }

    // Set up player input
    public void AssignPlayerInputDevice(PlayerInput playerInput)
    {
        this.playerInput = playerInput;

        // IMPORTANT:
        // This string must match the Input Actions asset:
        // Action Map = "Player"
        // Action = "Move"
        inputActionMove = playerInput.actions.FindAction("Player/Move");

        if (inputActionMove == null)
        {
            Debug.LogError(
                $"{name}: Could not find InputAction 'Player/Move'. " +
                $"Check your Input Actions asset action map + action names."
            );
        }
    }

    // Assign player number on spawn
    public void AssignPlayerNumber(int playerNumber)
    {
        PlayerNumber = playerNumber;
    }

    private void Update()
    {
        // Read movement every frame (smooth input), apply in FixedUpdate
        if (inputActionMove != null)
            moveInput = inputActionMove.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        if (Rigidbody2D == null)
        {
            Debug.Log($"{name}'s {nameof(PlayerController)}.{nameof(Rigidbody2D)} is null.");
            return;
        }

        // Normalize so diagonal movement isn't faster
        Vector2 desired = moveInput;
        if (desired.sqrMagnitude > 1f)
            desired = desired.normalized;

        // Velocity-based movement = clean for tilemap collisions
        Rigidbody2D.linearVelocity = desired * MoveSpeed;
    }

    private void OnValidate()
    {
        Reset();
    }

    private void Reset()
    {
        if (Rigidbody2D == null)
            Rigidbody2D = GetComponent<Rigidbody2D>();
        if (SpriteRenderer == null)
            SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
