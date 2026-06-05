using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private int difficultyLevel = 1; // Default difficulty level
    [SerializeField] private TextMeshProUGUI difficultyText; // Reference to the TextMeshProUGUI component for displaying difficulty
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
