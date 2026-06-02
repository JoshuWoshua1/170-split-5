using UnityEngine;
using UnityEngine.UI;

public class TelegraphTesting : MonoBehaviour
{
    [SerializeField] private Button[] telegraphButtons;
    [SerializeField] private TelegraphSystem telegraphSystem;

    private void Start()
    {
        if (telegraphSystem == null)
        {
            telegraphSystem = FindFirstObjectByType<TelegraphSystem>();
        }

        if (telegraphButtons != null)
        {
            for (int i = 0; i < telegraphButtons.Length; i++)
            {
                telegraphButtons[i].onClick.AddListener(PlayTelegraphSequence);
            }
        }
    }

    private void OnDestroy()
    {
        if (telegraphButtons != null)
        {
            for (int i = 0; i < telegraphButtons.Length; i++)
            {
                telegraphButtons[i].onClick.RemoveListener(PlayTelegraphSequence);
            }
        }
    }

    private void PlayTelegraphSequence()
    {
        if (telegraphSystem == null)
        {
            Debug.LogWarning("TelegraphTesting: No TelegraphSystem was assigned or found in the scene.");
            return;
        }

        telegraphSystem.PlayTelegraphSequence();
    }
}
