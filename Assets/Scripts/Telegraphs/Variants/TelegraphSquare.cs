using UnityEngine;
using System.Collections;

public class TelegraphSquare : Telegraph
{
    [SerializeField] private float sideLength;

    // Ask chat to group these once code is done --------------------------------------------------
    [Header("Resize Settings")]
    [SerializeField] private SizeChangeMode sizeChangeMode;
    [SerializeField] private float sizeChangeSpeed;
    [SerializeField] private float maxSideLength;

    [Header("Stepped Settings")]
    [SerializeField] private int stepCount = 3;
    [SerializeField] private float timeBetweenSteps = 0.15f;

    [Header("Last Moment Settings")]
    [SerializeField] private float lastMomentDelay = 0.5f;
    // Ask chat to group these once code is done --------------------------------------------------

    protected override void ApplyShapeSettings(TelegraphShapeSettings shape)
    {
        if (shape == null)
        {
            return;
        }

        sideLength = shape.square.sideLength;
        Rescale(Vector3.one * sideLength);
    }

    protected override IEnumerator SizeChange()
    {
        TelegraphSizeChangeSettings requestSize = GetSpawnRequest()?.sizeChange;
        SizeChangeMode mode = requestSize != null ? requestSize.mode : sizeChangeMode;
        float speed = requestSize != null ? requestSize.sizeChangeSpeed : sizeChangeSpeed;
        float targetSideLength = requestSize != null ? requestSize.maxPrimaryValue : maxSideLength;
        int steps = requestSize != null ? requestSize.stepCount : stepCount;
        float stepDelay = requestSize != null ? requestSize.timeBetweenSteps : timeBetweenSteps;
        float finalDelay = requestSize != null ? requestSize.lastMomentDelay : lastMomentDelay;

        if (mode == SizeChangeMode.Gradual)
        {
            float initialSideLength = sideLength;
            float elapsed = 0f;
            while (elapsed < speed)
            {
                sideLength = Mathf.Lerp(initialSideLength, targetSideLength, elapsed / speed);
                elapsed += Time.deltaTime;
                Rescale(Vector3.one * sideLength); // scale the telegraph object to match the side length
                yield return null;
            }
            sideLength = targetSideLength; // ensure it ends at max side length
            Rescale(Vector3.one * sideLength); // scale the telegraph object to match the side length
        }
        else if (mode == SizeChangeMode.LastMoment)
        {
            yield return new WaitForSeconds(finalDelay);
            sideLength = targetSideLength;
            Rescale(Vector3.one * sideLength); // scale the telegraph object to match the side length
        }
        else if (mode == SizeChangeMode.Stepped)
        {
            float stepIncrement = (targetSideLength - sideLength) / Mathf.Max(1, steps);
            for (int i = 0; i < Mathf.Max(1, steps); i++)
            {
                sideLength += stepIncrement;
                yield return new WaitForSeconds(stepDelay);
                Rescale(Vector3.one * sideLength); // scale the telegraph object to match the side length
            }
            sideLength = targetSideLength; // ensure it ends at max side length
            Rescale(Vector3.one * sideLength); // scale the telegraph object to match the side length
        }
    }

    protected override bool IsPlayerInTelegraph(Transform playerTransform)
    {
        float half = sideLength / 2f;
        Vector3 localPlayerPos = playerTransform.position - transform.position;
        //Debug.Log($"Local Player Position: {localPlayerPos}, Half Side Length: {half}");
        return Mathf.Abs(localPlayerPos.x) <= half && Mathf.Abs(localPlayerPos.z) <= half;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(sideLength, 1f, sideLength));
    }
}
