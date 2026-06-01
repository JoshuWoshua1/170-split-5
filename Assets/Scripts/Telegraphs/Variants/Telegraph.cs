using UnityEngine;
using System.Collections;

public class Telegraph : MonoBehaviour
{
    [SerializeField] private float telegraphDuration;
    [SerializeField] private int damage;
    //[SerializeField] private Collider telegraphCollider;
    // do nothing, snapshot player location upon the end of telegraph duration and apply damage if player is within collider bounds at that moment
    [SerializeField] private MeshRenderer telegraph;
    [SerializeField] private ParticleSystem hitParticle;
    [SerializeField] private Color ResolutionColor;
    [SerializeField] private Color ResolutionFillColor;
    void Start()
    {
        Debug.LogWarning("DebugCheck");
        StartCoroutine(TelegraphLifecycle());
    }

    private IEnumerator TelegraphLifecycle()
    {
        Debug.Log("Telegraph started.");
        telegraph.enabled = true;
        
        yield return new WaitForSeconds(telegraphDuration-(telegraphDuration / 2));
        StartCoroutine(ColorChange());

        yield return new WaitForSeconds(telegraphDuration / 2);
        hitParticle.Play();
        SnapshotDamage();
        telegraph.enabled = false; // hide telegraph after hit

        yield return new WaitForSeconds(5f); // allow time for hit particle to play before destroying telegraph object
        Destroy(gameObject);
    }

    private IEnumerator ColorChange()
    {
        float elapsedTime = 0f;
        Color initialFillColor = telegraph.material.GetColor("_FillColor"); // Assuming the shader uses _FillColor for the fill color
        Color targetFillColor = ResolutionFillColor; // Use the serialized ResolutionFillColor
        Color initialColor = telegraph.material.GetColor("_BaseColor"); // Assuming the shader uses _BaseColor for the main color
        Color targetColor = ResolutionColor; // Use the serialized ResolutionColor

        while (elapsedTime < (telegraphDuration / 2)) // Change color over the second half of the telegraph duration
        {
            Color currentFillColor = Color.Lerp(initialFillColor, targetFillColor, elapsedTime / (telegraphDuration / 2));
            Color currentColor = Color.Lerp(initialColor, targetColor, elapsedTime / (telegraphDuration / 2));
            telegraph.material.SetColor("_BaseColor", currentColor); // Assuming the shader uses _BaseColor for the main color
            telegraph.material.SetColor("_FillColor", currentFillColor); // Assuming the shader uses _FillColor for the fill color
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        telegraph.material.SetColor("_BaseColor", targetColor); // Ensure it ends on the target color
        telegraph.material.SetColor("_FillColor", targetFillColor); // Ensure it ends on the target fill color
    }

    protected virtual bool IsPlayerInTelegraph(Transform playerTransform)
    {
        // logic in variant classes
        return true; // Assume player is always in the telegraph for testing
    }

    private void SnapshotDamage()
    {
        if (IsPlayerInTelegraph(TempPlayer.Instance.transform))
        {
            Debug.Log($"Applying {damage} damage to player.");
        }
    }
}
