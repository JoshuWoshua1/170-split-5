using UnityEngine;
using UnityEngine.UI;

public class TelegraphTesting : MonoBehaviour
{
    [SerializeField] private Button[] telegraphButtons;
    [SerializeField] private Telegraph[] telegraphPrefabs;
    //[SerializeField] private TelegraphSpawnRequest telegraphRequest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (telegraphButtons != null)
        {
            for (int i = 0; i < telegraphButtons.Length; i++)
            {
                int index = i; // Capture the current index for the lambda
                telegraphButtons[i].onClick.AddListener(() => SpawnTelegraph(index));
            }
        }
    }

    private void OnDestroy()
    {
        if (telegraphButtons != null)
        {
            for (int i = 0; i < telegraphButtons.Length; i++)
            {
                int index = i; // Capture the current index for the lambda
                telegraphButtons[i].onClick.RemoveListener(() => SpawnTelegraph(index));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTelegraph(int index)
    {
        if (telegraphPrefabs == null || index < 0 || index >= telegraphPrefabs.Length)
        {
            Debug.LogWarning("TelegraphTesting: telegraphPrefab is not assigned or index is out of range.");
            return;
        }

        Vector3 spawnPosition = TempPlayer.Instance.transform.position;
        Instantiate(telegraphPrefabs[index], spawnPosition, Quaternion.identity);
    }
}
