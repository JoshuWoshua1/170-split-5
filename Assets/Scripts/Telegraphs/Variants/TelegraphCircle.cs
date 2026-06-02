using System.Collections;
using UnityEngine;

public enum SizeChangeMode
{
    None,
    Gradual,
    LastMoment,
    Stepped
}
public enum CircleCascadeShape
{
    Circle,
    Donut
}

public class TelegraphCircle : Telegraph
{
    [SerializeField] private float radius;

    // Ask chat to group these once code is done --------------------------------------------------
    [Header("Size Change Settings")]
    [SerializeField] private SizeChangeMode sizeChangeMode;
    [SerializeField] private float sizeChangeSpeed;
    [SerializeField] private float maxRadius;

    [Header("Stepped Settings")]
    [SerializeField] private int stepCount = 3;
    [SerializeField] private float timeBetweenSteps = 0.15f;

    [Header("Last Moment Settings")]
    [SerializeField] private float lastMomentDelay = 0.5f;
    // Ask chat to group these once code is done --------------------------------------------------

    [Header("Cascading Settings")]
    [SerializeField] private bool cascades;
    [SerializeField] private CircleCascadeShape cascadeShape;
    [SerializeField] private float cascadeDelay = 1f;
    [SerializeField] private int cascadeCount = 3;
    [SerializeField] private float cascadeRadiusIncrement = 1f;

    protected override void SetupTelegraph()
    {
        base.SetupTelegraph();
        /*if (cascades)
        {
            StartCoroutine(SpawnCascades());
        }*/
    }

    protected override void ApplyShapeSettings(TelegraphShapeSettings shape)
    {
        if (shape == null)
        {
            return;
        }

        radius = shape.circle.radius;
        Rescale(Vector3.one * radius * 2f);
    }

    protected override IEnumerator SizeChange()
    {
        if (sizeChangeMode == SizeChangeMode.Gradual)
        {
            float initialRadius = radius;
            float elapsed = 0f;
            while (elapsed < sizeChangeSpeed)
            {
                radius = Mathf.Lerp(initialRadius, maxRadius, elapsed / sizeChangeSpeed);
                elapsed += Time.deltaTime;
                Rescale(Vector3.one * radius * 2f); // scale the telegraph object to match the radius (diameter)
                yield return null;
            }
            radius = maxRadius; // ensure it ends at max radius
            Rescale(Vector3.one * radius * 2f); // scale the telegraph object to match the radius (diameter)
        }
        else if (sizeChangeMode == SizeChangeMode.LastMoment)
        {
            yield return new WaitForSeconds(lastMomentDelay);
            radius = maxRadius;
            Rescale(Vector3.one * radius * 2f); // scale the telegraph object to match the radius (diameter)
        }
        else if (sizeChangeMode == SizeChangeMode.Stepped)
        {
            float stepIncrement = (maxRadius - radius) / stepCount;
            for (int i = 0; i < stepCount; i++)
            {
                radius += stepIncrement;
                yield return new WaitForSeconds(timeBetweenSteps);
                Rescale(Vector3.one * radius * 2f); // scale the telegraph object to match the radius (diameter)
            }
            radius = maxRadius; // ensure it ends at max radius
            Rescale(Vector3.one * radius * 2f); // scale the telegraph object to match the radius (diameter)
        }
    }

    protected override bool IsPlayerInTelegraph(Transform playerTransform)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        return distanceToPlayer <= radius;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
