using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Multiple instances of GameManager detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    [SerializeField] private float GameTimer = 30f; // max game time
    private float currentTime;
    public bool isGameOver;
    public bool vicoryAchieved;

    public void GameOver(bool playerDied = false)
    {
        isGameOver = true;
        Debug.Log("Game Over!");
        Time.timeScale = 0f; // Pause the game
        if (playerDied)
        {
            Debug.Log("Player has died. Game Over!"); // lose
        }
        if (currentTime <= 0f)
        {
            Debug.Log("Time's up! Game Over!"); // win
        }
    }

    void Start()
    {
        currentTime = GameTimer;
        isGameOver = false;
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
    }

}
