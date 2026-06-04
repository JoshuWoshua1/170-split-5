using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject VictoryPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";
    [SerializeField] private Image damageVignette;
    [SerializeField] private float damageVignetteFadeDuration = 0.4f;
    private GameManager gameManager;
    private SceneManager sceneManager;
    private TempPlayer playerController;
    private int currentHealth;
    private Coroutine damageVignetteFadeCoroutine;
    private void Start()
    {
        gameManager = GameManager.Instance;
        sceneManager = SceneManager.Instance;
        playerController = TempPlayer.Instance;
        currentHealth = playerController.GetHealth();

        if (damageVignette != null)
        {
            Color color = damageVignette.color;
            color.a = 0f;
            damageVignette.color = color;
        }
    }

    private void Update()
    {
        if (gameManager != null && gameManager.victoryAchieved)
        {
            ShowVictoryPanel();
        }
        if (playerController != null)
        {
            int playerHealth = playerController.GetHealth();
            if (playerHealth < currentHealth)
            {
                PulseDamageVignette();
                currentHealth = playerHealth;
            }
        }
    }

    private void ShowVictoryPanel()
    {
        if (VictoryPanel != null)
        {
            VictoryPanel.SetActive(true);
        }
    }

    private void PulseDamageVignette()
    {
        if (damageVignette == null)
        {
            return;
        }

        if (damageVignetteFadeCoroutine != null)
        {
            StopCoroutine(damageVignetteFadeCoroutine);
        }

        damageVignetteFadeCoroutine = StartCoroutine(FadeDamageVignette());
    }

    private IEnumerator FadeDamageVignette()
    {
        Color color = damageVignette.color;
        color.a = 1f;
        damageVignette.color = color;

        float elapsed = 0f;
        while (elapsed < damageVignetteFadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / damageVignetteFadeDuration);
            damageVignette.color = color;
            yield return null;
        }

        color.a = 0f;
        damageVignette.color = color;
        damageVignetteFadeCoroutine = null;
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
