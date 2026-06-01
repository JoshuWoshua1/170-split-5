using UnityEngine;
using System.Collections;

public enum InOutMode
{
    InFirst,
    OutFirst
}

public class TelegraphDonut : Telegraph
{
    [Header("Donut Shape")]
    [SerializeField] private float innerRadius = 1f;
    [SerializeField] private float outerRadius = 3f;

    [Header("in/out mode")]
    [SerializeField] private InOutMode inOutMode;
    [SerializeField] private float delayBetweenInOut = 1f;

    // Future flow (no logic yet):
    // 1) Draw donut telegraph visuals
    // 2) Apply selected size mode
    // 3) Trigger optional cascades
    protected override bool IsPlayerInTelegraph(Transform playerTransform)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        return distanceToPlayer >= innerRadius && distanceToPlayer <= outerRadius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, outerRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, innerRadius);
    }
}
