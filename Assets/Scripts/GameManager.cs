using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple instances of GameManager detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    [SerializeField] private TMPro.TextMeshProUGUI debugTimerText; // Assign in inspector
    [SerializeField] private bool enableDebugTimer = false; // Toggle in inspector to show/hide timer text
    [SerializeField] private float GameTimer = 30f; // max game time
    private float currentTime;
    public bool isGameOver;
    public bool victoryAchieved = false;

    public void GameOver(bool playerDied = false)
    {
        isGameOver = true;
        Debug.Log("Game Over!");
        if (playerDied)
        {
            Debug.Log("Player has died. Game Over!"); // lose
        }
        if (currentTime <= 0f)
        {
            victoryAchieved = true;
            Debug.Log("Time's up! Game Over!"); // win
        }
        Time.timeScale = 0f; // Pause the game
    }

    void Start()
    {
        currentTime = GameTimer;
        isGameOver = false;
        if (debugTimerText != null)
        {
            debugTimerText.gameObject.SetActive(enableDebugTimer);
        }
    }

    void Update()
    {
        if (isGameOver)
        {
            return;
        }

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            GameOver();
        }
        updateDebugTimerText();
    }

    private void updateDebugTimerText()
    {
        if (debugTimerText != null)
        {
            debugTimerText.text = $"{currentTime:F2}s";
        }
    }

}
