using UnityEngine;

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
