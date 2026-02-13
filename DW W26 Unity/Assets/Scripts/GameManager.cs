using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Winner { None, Bunnies, Foxes }
    public static Winner LastWinner = Winner.None;

    [Header("Match Timer")]
    [SerializeField] private float matchDuration = 120f; // seconds (change in inspector)
    private float timeRemaining;
    private bool timerRunning;

    [Header("Scenes")]
    [SerializeField] private string gameplaySceneName = "Dual Monitor Scene"; 
    [SerializeField] private string endSceneName = "EndScene";               

    public static GameManager Instance { get; private set; }

    [Header("Carrot Win Condition")]
    [SerializeField] private int totalCarrotsInLevel;
    [SerializeField] private int depositedCarrots;

    [Header("Fox Win Condition")]
    [SerializeField] private int rabbitsOutCount;

    [Header("Rabbit Respawn")]
    [SerializeField] private Transform rabbitRespawnPoint;

    [Header("State")]
    public bool gameEnded;

    SetTimers timers;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Safety: if hit Play directly in gameplay scene
        if (SceneManager.GetActiveScene().name == gameplaySceneName)
        {
            InitializeGameplayState();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[GameManager] Scene loaded: '{scene.name}' (expecting gameplay: '{gameplaySceneName}')");

        if (scene.name != gameplaySceneName) return;

        InitializeGameplayState();
    }

    private void InitializeGameplayState()
    {
        gameEnded = false;
        depositedCarrots = 0;
        rabbitsOutCount = 0;
        LastWinner = Winner.None;

        // Count carrots present at the start of the match
        totalCarrotsInLevel = GameObject.FindGameObjectsWithTag("Carrot").Length;

        // Start timer
        timeRemaining = matchDuration;
        timerRunning = true;

        Debug.Log($"[GameManager] Gameplay initialized. Total carrots: {totalCarrotsInLevel}, Timer: {matchDuration}s");
    }

    private void Update()
    {
        if (!timerRunning || gameEnded) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerRunning = false;

            Debug.Log("TIME UP! Foxes win.");
            FoxWin();
        }

        timers.SetTimersText(timeRemaining); // hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh
    }

    // ----------------------------
    // BUNNY WIN
    // ----------------------------
    public void AddDepositedCarrots(int amount)
    {
        if (gameEnded) return;

        depositedCarrots += amount;
        Debug.Log($"Deposited carrots: {depositedCarrots}/{totalCarrotsInLevel}");

        if (depositedCarrots >= totalCarrotsInLevel && totalCarrotsInLevel > 0)
        {
            BunnyWin();
        }
    }

    private void BunnyWin()
    {
        if (gameEnded) return;

        gameEnded = true;
        timerRunning = false;
        LastWinner = Winner.Bunnies;

        Debug.Log("BUNNIES WIN! Loading end scene...");
        SceneManager.LoadScene(endSceneName);
    }

    // ----------------------------
    // FOX WIN
    // ----------------------------
    public void CheckFoxWin()
    {
        if (gameEnded) return;

        int outCount = 0;
        var players = GameObject.FindGameObjectsWithTag("Player");

        foreach (var p in players)
        {
            var role = p.GetComponent<PlayerRole>();
            if (role == null || role.role != PlayerRole.Role.Rabbit) continue;

            var lives = p.GetComponent<PlayerLives>();
            if (lives != null && lives.IsDead) outCount++;
        }

        rabbitsOutCount = outCount;

        if (rabbitsOutCount >= 2)
        {
            FoxWin();
        }
    }

    private void FoxWin()
    {
        if (gameEnded) return;

        gameEnded = true;
        timerRunning = false;
        LastWinner = Winner.Foxes;

        Debug.Log("FOXES WIN! Loading end scene...");
        SceneManager.LoadScene(endSceneName);
    }

    // ----------------------------
    // RABBIT RESPAWN + STUN
    // ----------------------------
    public void RespawnRabbit(Transform rabbit)
    {
        if (rabbitRespawnPoint == null || rabbit == null) return;

        rabbit.position = rabbitRespawnPoint.position;

        var rb = rabbit.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;
    }

    public void StunThenRespawnRabbit(GameObject rabbit, float stunSeconds = 1f)
    {
        if (rabbit == null) return;
        StartCoroutine(StunRespawnRoutine(rabbit, stunSeconds));
    }

    private System.Collections.IEnumerator StunRespawnRoutine(GameObject rabbit, float stunSeconds)
    {
        var controller = rabbit.GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        var rb = rabbit.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(stunSeconds);

        RespawnRabbit(rabbit.transform);

        var lives = rabbit.GetComponent<PlayerLives>();
        bool isDead = (lives != null && lives.IsDead);

        if (!isDead && controller != null)
            controller.enabled = true;
    }

    // Optional getter if we want to show UI timer later
    public float GetTimeRemaining() => timeRemaining;
}
