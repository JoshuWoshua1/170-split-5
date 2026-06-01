using UnityEngine;

public class TelegraphCone : Telegraph
{
    [SerializeField] private float radius;
    [SerializeField] private float angle;

    // Ask chat to group these once code is done --------------------------------------------------
    [Header("Size Change Settings")]
    [SerializeField] private SizeChangeMode sizeChangeMode;
    [SerializeField] private float sizeChangeSpeed;
    [SerializeField] private float maxRadius;
    [SerializeField] private float maxAngle;

    [Header("Stepped Settings")]
    [SerializeField] private int stepCount = 3;
    [SerializeField] private float timeBetweenSteps = 0.15f;

    [Header("Last Moment Settings")]
    [SerializeField] private float lastMomentDelay = 0.5f;
    // Ask chat to group these once code is done --------------------------------------------------

    /* 
    Requires me to make pineapple slices instead of donuts, will do if i have time.

    [Header("Cascading Settings")]
    [SerializeField] private bool cascades;
    [SerializeField] private float cascadeDelay = 1f;
    [SerializeField] private int cascadeCount = 3;
    */

    protected override bool IsPlayerInTelegraph(Transform playerTransform)
    {
        Vector3 toPlayer = playerTransform.position - transform.position;
        float distanceToPlayer = toPlayer.magnitude;

        if (distanceToPlayer > radius)
            return false;

        float angleToPlayer = Vector3.Angle(transform.forward, toPlayer);
        return angleToPlayer <= angle / 2f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 forward = transform.forward * radius;
        Vector3 rightBoundary = Quaternion.Euler(0, angle / 2f, 0) * forward;
        Vector3 leftBoundary = Quaternion.Euler(0, -angle / 2f, 0) * forward;

        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + forward/1.5f);
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
