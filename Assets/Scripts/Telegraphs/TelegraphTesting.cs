using UnityEngine;
using UnityEngine.UI;

public class TelegraphTesting : MonoBehaviour
{
    [SerializeField] private Button spawnTelegraph;
    [SerializeField] private Telegraph telegraphPrefab;
    //[SerializeField] private TelegraphSpawnRequest telegraphRequest;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (spawnTelegraph != null)
        {
            spawnTelegraph.onClick.AddListener(SpawnTelegraph);
        }
    }

    private void OnDestroy()
    {
        if (spawnTelegraph != null)
        {
            spawnTelegraph.onClick.RemoveListener(SpawnTelegraph);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnTelegraph()
    {
        if (telegraphPrefab == null)
        {
            Debug.LogWarning("TelegraphTesting: telegraphPrefab is not assigned.");
            return;
        }

        Vector3 spawnPosition = TempPlayer.Instance.transform.position;
        Instantiate(telegraphPrefab, spawnPosition, Quaternion.identity);
    }
}
