using UnityEngine;
using System.Collections;

public enum InOutMode
{
    InFirst,
    OutFirst
}

public class TelegraphDonut : Telegraph
{
    private const float ShaderOuterRadius = 0.5f;

    [Header("Donut Shape")]
    [SerializeField] private float innerRadius = 1f;
    [SerializeField] private float outerRadius = 3f;

    [Header("in/out mode")]
    [SerializeField] private InOutMode inOutMode;
    [SerializeField] private float delayBetweenInOut = 1f;

    protected override void ApplyShapeSettings(TelegraphShapeSettings shape)
    {
        if (shape == null)
        {
            return;
        }

        innerRadius = Mathf.Max(0f, shape.donut.innerRadius);
        outerRadius = Mathf.Max(innerRadius, shape.donut.outerRadius);
        float diameter = outerRadius * 2f;
        Rescale(new Vector3(diameter/2, 1f, diameter/2));
    }

    protected override void Rescale(Vector3 newScale)
    {
        transform.localScale = newScale;

        float normalizedInnerRadius = outerRadius <= 0f ? 0f : (innerRadius / outerRadius) * ShaderOuterRadius;
        telegraph.material.SetFloat("_InnerRadius", normalizedInnerRadius);
        telegraph.material.SetFloat("_OuterRadius", ShaderOuterRadius);
    }

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
