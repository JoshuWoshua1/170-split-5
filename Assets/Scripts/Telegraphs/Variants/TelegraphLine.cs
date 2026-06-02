using UnityEngine;

public class TelegraphLine : Telegraph
{
    [SerializeField] private float lineLength;
    [SerializeField] private float lineWidth;

    // Ask chat to group these once code is done --------------------------------------------------
    [Header("Size Change Settings")]
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

    private void OnDrawGizmosSelected()
    {   
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(lineLength, 1f, lineWidth));
        Gizmos.matrix = oldMatrix;
    }
}
