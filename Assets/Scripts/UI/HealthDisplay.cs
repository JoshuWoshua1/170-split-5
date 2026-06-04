using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] private GameObject[] heartIcons; // Assign in inspector

     void Start()
    {
        UpdateHealthDisplay();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateHealthDisplay();
    }
    private void UpdateHealthDisplay()
    {
        int currentHealth = TempPlayer.Instance.GetHealth();

        for (int i = 0; i < heartIcons.Length; i++)
        {
            if (i < currentHealth)
            {
                heartIcons[i].SetActive(true);
            }
            else
            {
                heartIcons[i].SetActive(false);
            }
        }
    }
}
