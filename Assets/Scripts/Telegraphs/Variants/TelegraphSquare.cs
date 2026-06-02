using UnityEngine;
using System.Collections;

public class TelegraphSquare : Telegraph
{
    [SerializeField] private float sideLength;

    // Ask chat to group these once code is done --------------------------------------------------
    [Header("Size Change Settings")]
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
        if (sizeChangeMode == SizeChangeMode.Gradual)
        {
            float initialSideLength = sideLength;
            float elapsed = 0f;
            while (elapsed < sizeChangeSpeed)
            {
                sideLength = Mathf.Lerp(initialSideLength, maxSideLength, elapsed / sizeChangeSpeed);
                elapsed += Time.deltaTime;
                Rescale(Vector3.one * sideLength); // scale the telegraph object to match the side length
                yield return null;
            }
            sideLength = maxSideLength; // ensure it ends at max side length
            Rescale(Vector3.one * sideLength); // scale the telegraph object to match the side length
        }
        else if (sizeChangeMode == SizeChangeMode.LastMoment)
        {
            yield return new WaitForSeconds(lastMomentDelay);
            sideLength = maxSideLength;
            Rescale(Vector3.one * sideLength); // scale the telegraph object to match the side length
        }
        else if (sizeChangeMode == SizeChangeMode.Stepped)
        {
            float stepIncrement = (maxSideLength - sideLength) / stepCount;
            for (int i = 0; i < stepCount; i++)
            {
                sideLength += stepIncrement;
                yield return new WaitForSeconds(timeBetweenSteps);
                Rescale(Vector3.one * sideLength); // scale the telegraph object to match the side length
            }
            sideLength = maxSideLength; // ensure it ends at max side length
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
