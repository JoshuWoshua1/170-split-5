using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private int difficultyLevel = 1; // Default difficulty level
    [SerializeField] private TextMeshProUGUI difficultyText; // Reference to the TextMeshProUGUI component for displaying difficulty

    [Header("Scene Management")]
    [SerializeField] private string EasyScene = "Easy"; // Name of the easy scene
    [SerializeField] private string MediumScene = "Medium"; // Name of the medium scene
    [SerializeField] private string HardScene = "Hard"; // Name of the hard scene
    private SceneManager sceneManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sceneManager = SceneManager.Instance;
        DifficultyTextUpdate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDifficultyLevel(int level)
    {
        difficultyLevel = level;
        Debug.Log("Difficulty level set to: " + difficultyLevel);
        // You can add additional logic here to apply the difficulty setting in your game
        DifficultyTextUpdate();
    }

    private void DifficultyTextUpdate()
    {
        if (difficultyText != null)
        {
            if (difficultyLevel == 1)
            {
                difficultyText.text = "Mode: Easy";
            }
            else if (difficultyLevel == 2)
            {
                difficultyText.text = "Mode: Medium";
            }
            else if (difficultyLevel == 3)
            {
                difficultyText.text = "Mode: Hard";
            }
        }
    }

    public void StartGame()
    {
        // Logic to start the game goes here
        Debug.Log("Start Game button clicked. Starting the game...");
        if (difficultyLevel == 1)
        {
            Debug.Log("Starting game on Easy mode.");
            sceneManager.LoadScene(EasyScene);
        }
        else if (difficultyLevel == 2)
        {
            Debug.Log("Starting game on Medium mode.");
            sceneManager.LoadScene(MediumScene);
        }
        else if (difficultyLevel == 3)
        {
            Debug.Log("Starting game on Hard mode.");
            sceneManager.LoadScene(HardScene);
        }
        // You can load a new scene or enable game objects to transition to the gameplay state
    }
}
