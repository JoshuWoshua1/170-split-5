using UnityEngine;
using System.Collections;

public class Telegraph : MonoBehaviour
{
    [SerializeField] private float telegraphDuration;
    [SerializeField] private int damage;
    //[SerializeField] private Collider telegraphCollider;
    // do nothing, snapshot player location upon the end of telegraph duration and apply damage if player is within collider bounds at that moment
    [SerializeField] private ParticleSystem telegraphParticle;
    [SerializeField] private ParticleSystem hitParticle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.LogWarning("DebugCheck");
        StartCoroutine(TelegraphLifecycle());
    }

    private IEnumerator TelegraphLifecycle()
    {
        Debug.Log("Telegraph started.");
        telegraphParticle.Play();
        
        yield return new WaitForSeconds(telegraphDuration-1f);
        telegraphParticle.Stop();

        yield return new WaitForSeconds(1f);
        hitParticle.Play();
        SnapshotDamage();

        yield return new WaitForSeconds(5f); // allow time for hit particle to play before destroying telegraph object
        Destroy(gameObject);
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
