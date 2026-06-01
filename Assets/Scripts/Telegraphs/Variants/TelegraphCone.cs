using UnityEngine;
using System.Collections;

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

    protected override IEnumerator SizeChange()
    {
        if (sizeChangeMode == SizeChangeMode.Gradual)
        {
            float initialAngle = angle;
            float initialRadius = radius;
            float elapsed = 0f;
            while (elapsed < sizeChangeSpeed)
            {
                radius = Mathf.Lerp(initialRadius, maxRadius, elapsed / sizeChangeSpeed);
                angle = Mathf.Lerp(initialAngle, maxAngle, elapsed / sizeChangeSpeed);
                elapsed += Time.deltaTime;
                Rescale(Vector3.one * radius);
                yield return null;
            }
            radius = maxRadius; // ensure it ends at max radius
            angle = maxAngle; // ensure it ends at max angle
            Rescale(Vector3.one * radius);
        }
        else if (sizeChangeMode == SizeChangeMode.LastMoment)
        {
            yield return new WaitForSeconds(lastMomentDelay);
            radius = maxRadius;
            angle = maxAngle;
            Rescale(Vector3.one * radius);
        }
        else if (sizeChangeMode == SizeChangeMode.Stepped)
        {
            float stepIncrement = (maxRadius - radius) / stepCount;
            for (int i = 0; i < stepCount; i++)
            {
                angle += (maxAngle - angle) / stepCount;
                radius += stepIncrement;
                yield return new WaitForSeconds(timeBetweenSteps);
                Rescale(Vector3.one * radius);
            }
            angle = maxAngle; // ensure it ends at max angle
            radius = maxRadius; // ensure it ends at max radius
            Rescale(Vector3.one * radius);
        }
    }

    protected override void Rescale(Vector3 newScale)
    {
        transform.localScale = newScale;
        telegraph.material.SetFloat("_Angle", angle/2f); // Assuming the shader uses _Angle for the cone angle
    }

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
