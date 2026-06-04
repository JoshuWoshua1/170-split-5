using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject VictoryPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    private GameManager gameManager;
    private SceneManager sceneManager;
    private void Start()
    {
        gameManager = GameManager.Instance;
        sceneManager = SceneManager.Instance;
    }

    private void Update()
    {
        if (gameManager != null && gameManager.victoryAchieved)
        {
            ShowVictoryPanel();
        }
    }

    private void ShowVictoryPanel()
    {
        if (VictoryPanel != null)
        {
            VictoryPanel.SetActive(true);
        }
    }

    #region Buttons

    public void OnRestartButtonPressed()
    {
        sceneManager.ReloadCurrentScene();
    }

    public void OnHomeButtonPressed()
    {
        sceneManager.LoadScene(mainMenuSceneName);
    }

    #endregion
}
