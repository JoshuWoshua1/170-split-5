using UnityEngine;

public class EffectDeleteTimer : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f;
    private float timer;
    private Renderer cachedRenderer;
    private Color initialColor;

    private void Awake()
    {
        cachedRenderer = GetComponent<Renderer>();
        if (cachedRenderer != null)
        {
            initialColor = cachedRenderer.material.color;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (cachedRenderer != null)
        {
            float normalized = Mathf.Clamp01(timer / Mathf.Max(0.0001f, lifetime));
            Color faded = initialColor;
            faded.a = Mathf.Lerp(initialColor.a, 0f, normalized);
            cachedRenderer.material.color = faded;
        }

        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
