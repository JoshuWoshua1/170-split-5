using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Multiple instances of SceneManager detected. Destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f; // Ensure time scale is reset when loading a new scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        Time.timeScale = 1f; // Ensure time scale is reset when reloading the scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
