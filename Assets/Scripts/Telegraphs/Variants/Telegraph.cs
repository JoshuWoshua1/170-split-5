using UnityEngine;
using System.Collections;

public class Telegraph : MonoBehaviour
{
    [SerializeField] private float telegraphDuration;
    [SerializeField] private int damage;
    //[SerializeField] private Collider telegraphCollider;
    // do nothing, snapshot player location upon the end of telegraph duration and apply damage if player is within collider bounds at that moment
    public MeshRenderer telegraph;
    public ParticleSystem hitParticle;
    [SerializeField] private Color InitialColor;
    [SerializeField] private Color InitialFillColor;
    [SerializeField] private Color ResolutionColor;
    [SerializeField] private Color ResolutionFillColor;

    private TelegraphSpawnRequest spawnRequest;
    private TelegraphSystem telegraphSystem;

    public void ApplySpawnRequest(TelegraphSpawnRequest request, TelegraphSystem ownerSystem)
    {
        if (request == null)
        {
            return;
        }

        spawnRequest = request;
        telegraphSystem = ownerSystem;

        telegraphDuration = Mathf.Max(0f, request.duration);
        damage = request.damage;

        ApplyShapeSettings(request.shape);
    }

    protected virtual void ApplyShapeSettings(TelegraphShapeSettings shape)
    {
        // Implemented by shape variants.
    }

    protected TelegraphSpawnRequest GetSpawnRequest()
    {
        return spawnRequest;
    }
    void Start()
    {
        Debug.LogWarning("DebugCheck");
        telegraph.material.SetColor("_BaseColor", InitialColor);
        telegraph.material.SetColor("_FillColor", InitialFillColor);
        SetupTelegraph();
        StartCoroutine(TelegraphLifecycle());
    }

    protected virtual void SetupTelegraph()
    {
        // default does nothing, can be overridden by variants for additional setup
    }

    private IEnumerator TelegraphLifecycle()
    {
        Debug.Log("Telegraph started.");
        StartCoroutine(SizeChange());
        StartCoroutine(Cascade());
        telegraph.enabled = true;
        
        yield return new WaitForSeconds(telegraphDuration-(telegraphDuration / 2));
        StartCoroutine(ColorChange());

        yield return new WaitForSeconds(telegraphDuration / 2);
        hitParticle.Play();
        SnapshotDamage();
        telegraph.enabled = false; // hide telegraph after hit

        yield return new WaitForSeconds(5f); // allow time for hit particle to play before destroying telegraph object
        Destroy(gameObject);
    }

    private IEnumerator ColorChange()
    {
        float elapsedTime = 0f;
        Color initialFillColor = telegraph.material.GetColor("_FillColor"); // Assuming the shader uses _FillColor for the fill color
        Color targetFillColor = ResolutionFillColor; // Use the serialized ResolutionFillColor
        Color initialColor = telegraph.material.GetColor("_BaseColor"); // Assuming the shader uses _BaseColor for the main color
        Color targetColor = ResolutionColor; // Use the serialized ResolutionColor

        while (elapsedTime < (telegraphDuration / 2)) // Change color over the second half of the telegraph duration
        {
            Color currentFillColor = Color.Lerp(initialFillColor, targetFillColor, elapsedTime / (telegraphDuration / 2));
            Color currentColor = Color.Lerp(initialColor, targetColor, elapsedTime / (telegraphDuration / 2));
            telegraph.material.SetColor("_BaseColor", currentColor); // Assuming the shader uses _BaseColor for the main color
            telegraph.material.SetColor("_FillColor", currentFillColor); // Assuming the shader uses _FillColor for the fill color
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        telegraph.material.SetColor("_BaseColor", targetColor); // Ensure it ends on the target color
        telegraph.material.SetColor("_FillColor", targetFillColor); // Ensure it ends on the target fill color
    }

    protected virtual IEnumerator SizeChange()
    {
        yield break; // default does nothing, only used for telegraph variants that change size over time
    }

    protected virtual IEnumerator Cascade()
    {
        if (spawnRequest == null || telegraphSystem == null || spawnRequest.cascade == null)
        {
            yield break;
        }

        if (!spawnRequest.cascade.cascades || spawnRequest.cascade.cascadeCount <= 1)
        {
            yield break;
        }

        yield return new WaitForSeconds(spawnRequest.cascade.cascadeDelay);

        TelegraphSpawnRequest nextRequest = CloneRequestForNextCascade(spawnRequest, transform.position);
        telegraphSystem.SpawnTelegraph(nextRequest);
    }

    protected virtual void Rescale(Vector3 newScale)
    {
        transform.localScale = newScale;
    }

    protected virtual bool IsPlayerInTelegraph(Transform playerTransform)
    {
        // logic in variant classes
        return true; // Assume player is always in the telegraph for testing
    }

    private void SnapshotDamage()
    {
        if (IsPlayerInTelegraph(TempPlayer.Instance.transform))
        {
            Debug.Log($"Applying {damage} damage to player.");
            TempPlayer.Instance.TakeDamage(damage);
        }
    }

    private TelegraphSpawnRequest CloneRequestForNextCascade(TelegraphSpawnRequest source, Vector3 position)
    {
        TelegraphSpawnRequest next = new TelegraphSpawnRequest();
        next.origin = source.origin;
        next.worldPosition = position;
        next.duration = source.duration;
        next.damage = source.damage;

        TelegraphShapeSettings baseShape = source.cascade.useCascadeShape
            ? source.cascade.cascadeShape
            : source.shape;

        CopyShapeSettings(baseShape, next.shape);
        ApplyCascadeSizeSteps(next.shape, source.cascade);

        next.cascade.cascades = source.cascade.cascades;
        next.cascade.cascadeDelay = source.cascade.cascadeDelay;
        next.cascade.cascadeCount = source.cascade.cascadeCount - 1;
        next.cascade.useCascadeShape = source.cascade.useCascadeShape;
        CopyShapeSettings(source.cascade.cascadeShape, next.cascade.cascadeShape);
        next.cascade.circleRadiusStep = source.cascade.circleRadiusStep;
        next.cascade.squareSideLengthStep = source.cascade.squareSideLengthStep;
        next.cascade.donutInnerRadiusStep = source.cascade.donutInnerRadiusStep;
        next.cascade.donutOuterRadiusStep = source.cascade.donutOuterRadiusStep;
        next.cascade.lineLengthStep = source.cascade.lineLengthStep;
        next.cascade.lineWidthStep = source.cascade.lineWidthStep;
        next.cascade.coneRadiusStep = source.cascade.coneRadiusStep;
        next.cascade.coneAngleStep = source.cascade.coneAngleStep;

        return next;
    }

    private static void CopyShapeSettings(TelegraphShapeSettings source, TelegraphShapeSettings destination)
    {
        if (source == null || destination == null)
        {
            return;
        }

        destination.shapeType = source.shapeType;
        destination.circle.radius = source.circle.radius;
        destination.square.sideLength = source.square.sideLength;
        destination.donut.innerRadius = source.donut.innerRadius;
        destination.donut.outerRadius = source.donut.outerRadius;
        destination.line.length = source.line.length;
        destination.line.width = source.line.width;
        destination.cone.radius = source.cone.radius;
        destination.cone.angle = source.cone.angle;
    }

    private static void ApplyCascadeSizeSteps(TelegraphShapeSettings shape, TelegraphCascadeSettings cascade)
    {
        if (shape == null || cascade == null)
        {
            return;
        }

        shape.circle.radius = Mathf.Max(0f, shape.circle.radius + cascade.circleRadiusStep);
        shape.square.sideLength = Mathf.Max(0f, shape.square.sideLength + cascade.squareSideLengthStep);
        shape.donut.innerRadius = Mathf.Max(0f, shape.donut.innerRadius + cascade.donutInnerRadiusStep);
        shape.donut.outerRadius = Mathf.Max(shape.donut.innerRadius, shape.donut.outerRadius + cascade.donutOuterRadiusStep);
        shape.line.length = Mathf.Max(0f, shape.line.length + cascade.lineLengthStep);
        shape.line.width = Mathf.Max(0f, shape.line.width + cascade.lineWidthStep);
        shape.cone.radius = Mathf.Max(0f, shape.cone.radius + cascade.coneRadiusStep);
        shape.cone.angle = Mathf.Clamp(shape.cone.angle + cascade.coneAngleStep, 0f, 360f);
    }
}
