using UnityEngine;
using System.Collections;

public class TelegraphLine : Telegraph
{
    [SerializeField] private float lineLength;
    [SerializeField] private float lineWidth;

    // Ask chat to group these once code is done --------------------------------------------------
    [Header("Resize Settings")]
    [SerializeField] private SizeChangeMode sizeChangeMode;
    [SerializeField] private float sizeChangeSpeed;
    [SerializeField] private float maxLineLength;
    [SerializeField] private float maxLineWidth;

    [Header("Stepped Settings")]
    [SerializeField] private int stepCount = 3;
    [SerializeField] private float timeBetweenSteps = 0.15f;

    [Header("Last Moment Settings")]
    [SerializeField] private float lastMomentDelay = 0.5f;
    // Ask chat to group these once code is done --------------------------------------------------

    [Header("Cascading Settings")]
    [SerializeField] private bool cascades;
    [SerializeField] private float cascadeDelay = 1f;
    [SerializeField] private int cascadeCount = 3;
    [SerializeField] private float cascadeSizeIncrement = 1f;

    protected override bool IsPlayerInTelegraph(Transform playerTransform)
    {
        float halfLength = lineLength / 2f;
        float halfWidth = lineWidth / 2f;
        Vector3 toPlayer = playerTransform.position - transform.position;
        Vector3 localPlayerPos = Quaternion.Inverse(transform.rotation) * toPlayer;
        return localPlayerPos.x >= -halfLength && localPlayerPos.x <= halfLength &&
               localPlayerPos.z >= -halfWidth && localPlayerPos.z <= halfWidth;
    }

    protected override void ApplyShapeSettings(TelegraphShapeSettings shape)
    {
        if (shape == null)
        {
            return;
        }

        lineLength = shape.line.length;
        lineWidth = shape.line.width;
        Rescale(new Vector3(lineLength/2, 1f, lineWidth*2));
    }

    protected override void Rescale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }

    protected override IEnumerator SizeChange()
    {
        TelegraphSizeChangeSettings requestSize = GetSpawnRequest()?.sizeChange;
        SizeChangeMode mode = requestSize != null ? requestSize.mode : sizeChangeMode;
        float speed = requestSize != null ? requestSize.sizeChangeSpeed : sizeChangeSpeed;
        float targetLength = requestSize != null ? requestSize.maxPrimaryValue : maxLineLength;
        float targetWidth = requestSize != null ? requestSize.maxSecondaryValue : maxLineWidth;
        int steps = requestSize != null ? requestSize.stepCount : stepCount;
        float stepDelay = requestSize != null ? requestSize.timeBetweenSteps : timeBetweenSteps;
        float finalDelay = requestSize != null ? requestSize.lastMomentDelay : lastMomentDelay;

        if (mode == SizeChangeMode.Gradual)
        {
            float initialLength = lineLength;
            float initialWidth = lineWidth;
            float elapsed = 0f;
            while (elapsed < speed)
            {
                lineLength = Mathf.Lerp(initialLength, targetLength, elapsed / speed);
                lineWidth = Mathf.Lerp(initialWidth, targetWidth, elapsed / speed);
                elapsed += Time.deltaTime;
                Rescale(new Vector3(lineLength / 2f, 1f, lineWidth * 2f));
                yield return null;
            }

            lineLength = targetLength;
            lineWidth = targetWidth;
            Rescale(new Vector3(lineLength / 2f, 1f, lineWidth * 2f));
        }
        else if (mode == SizeChangeMode.LastMoment)
        {
            yield return new WaitForSeconds(finalDelay);
            lineLength = targetLength;
            lineWidth = targetWidth;
            Rescale(new Vector3(lineLength / 2f, 1f, lineWidth * 2f));
        }
        else if (mode == SizeChangeMode.Stepped)
        {
            int safeSteps = Mathf.Max(1, steps);
            float lengthStep = (targetLength - lineLength) / safeSteps;
            float widthStep = (targetWidth - lineWidth) / safeSteps;

            for (int i = 0; i < safeSteps; i++)
            {
                lineLength += lengthStep;
                lineWidth += widthStep;
                yield return new WaitForSeconds(stepDelay);
                Rescale(new Vector3(lineLength / 2f, 1f, lineWidth * 2f));
            }

            lineLength = targetLength;
            lineWidth = targetWidth;
            Rescale(new Vector3(lineLength / 2f, 1f, lineWidth * 2f));
        }
    }

    private void OnDrawGizmosSelected()
    {   
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(lineLength, 1f, lineWidth));
        Gizmos.matrix = oldMatrix;
    }
}
