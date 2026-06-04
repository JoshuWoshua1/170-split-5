using UnityEngine;

public class EffectDeleteTimer : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    private float timer;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
