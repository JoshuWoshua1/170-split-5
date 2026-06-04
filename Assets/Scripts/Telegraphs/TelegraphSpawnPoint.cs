using UnityEngine;

public class TelegraphSpawnPoint : MonoBehaviour
{
    [Header("Placement")]
    [SerializeField] private bool useTransformAsOrigin = true;
    [SerializeField] private bool useTransformPosition = true;

    [Header("Spawn Request")]
    [SerializeField] private TelegraphSpawnRequest spawnRequest = new TelegraphSpawnRequest();

    [Header("Preview")]
    [SerializeField] private bool drawWhenSelectedOnly = true;
    [SerializeField] private bool previewFirstCascade;
    [SerializeField] private bool previewResizeResult = true;
    [SerializeField] private Color previewColor = new Color(1f, 0.2f, 0.2f, 1f);
    [SerializeField] private Color cascadePreviewColor = new Color(1f, 0.8f, 0.2f, 1f);
    [SerializeField] private Color resizePreviewColor = new Color(0.2f, 0.9f, 1f, 1f);

    public TelegraphSpawnRequest CreateSpawnRequest()
    {
        TelegraphSpawnRequest request = CloneRequest(spawnRequest);

        if (useTransformAsOrigin)
        {
            request.origin = transform;
        }

        if (useTransformPosition)
        {
            request.worldPosition = transform.position;
        }

        return request;
    }

    private void OnValidate()
    {
        if (spawnRequest == null)
        {
            spawnRequest = new TelegraphSpawnRequest();
        }

        if (spawnRequest.shape == null)
        {
            spawnRequest.shape = new TelegraphShapeSettings();
        }

        if (spawnRequest.sizeChange == null)
        {
            spawnRequest.sizeChange = new TelegraphSizeChangeSettings();
        }

        if (spawnRequest.cascade == null)
        {
            spawnRequest.cascade = new TelegraphCascadeSettings();
        }

        if (spawnRequest.cascade.cascadeShape == null)
        {
            spawnRequest.cascade.cascadeShape = new TelegraphShapeSettings();
        }

        spawnRequest.duration = Mathf.Max(0f, spawnRequest.duration);
        spawnRequest.shape.circle.radius = Mathf.Max(0f, spawnRequest.shape.circle.radius);
        spawnRequest.shape.square.sideLength = Mathf.Max(0f, spawnRequest.shape.square.sideLength);
        spawnRequest.shape.donut.innerRadius = Mathf.Max(0f, spawnRequest.shape.donut.innerRadius);
        spawnRequest.shape.donut.outerRadius = Mathf.Max(spawnRequest.shape.donut.innerRadius, spawnRequest.shape.donut.outerRadius);
        spawnRequest.shape.line.length = Mathf.Max(0f, spawnRequest.shape.line.length);
        spawnRequest.shape.line.width = Mathf.Max(0f, spawnRequest.shape.line.width);
        spawnRequest.shape.cone.radius = Mathf.Max(0f, spawnRequest.shape.cone.radius);
        spawnRequest.shape.cone.angle = Mathf.Clamp(spawnRequest.shape.cone.angle, 0f, 360f);

        spawnRequest.sizeChange.sizeChangeSpeed = Mathf.Max(0f, spawnRequest.sizeChange.sizeChangeSpeed);
        spawnRequest.sizeChange.maxPrimaryValue = Mathf.Max(0f, spawnRequest.sizeChange.maxPrimaryValue);
        spawnRequest.sizeChange.maxSecondaryValue = Mathf.Max(0f, spawnRequest.sizeChange.maxSecondaryValue);
        spawnRequest.sizeChange.stepCount = Mathf.Max(1, spawnRequest.sizeChange.stepCount);
        spawnRequest.sizeChange.timeBetweenSteps = Mathf.Max(0f, spawnRequest.sizeChange.timeBetweenSteps);
        spawnRequest.sizeChange.lastMomentDelay = Mathf.Max(0f, spawnRequest.sizeChange.lastMomentDelay);

        spawnRequest.cascade.cascadeDelay = Mathf.Max(0f, spawnRequest.cascade.cascadeDelay);
        spawnRequest.cascade.cascadeCount = Mathf.Max(1, spawnRequest.cascade.cascadeCount);
    }

    private void OnDrawGizmos()
    {
        if (drawWhenSelectedOnly)
        {
            return;
        }

        DrawPreviewGizmos();
    }

    private void OnDrawGizmosSelected()
    {
        DrawPreviewGizmos();
    }

    private void DrawPreviewGizmos()
    {
        if (spawnRequest == null || spawnRequest.shape == null)
        {
            return;
        }

        Vector3 position = useTransformPosition ? transform.position : spawnRequest.worldPosition;
        Transform originTransform = useTransformAsOrigin ? transform : spawnRequest.origin;
        Quaternion rotation = originTransform != null ? originTransform.rotation : transform.rotation;

        DrawShapeGizmo(spawnRequest.shape, position, rotation, previewColor);

        if (previewResizeResult)
        {
            DrawResizeResultGizmo(spawnRequest.shape, spawnRequest.sizeChange, position, rotation, resizePreviewColor);
        }

        if (!previewFirstCascade || spawnRequest.cascade == null || !spawnRequest.cascade.cascades)
        {
            return;
        }

        TelegraphShapeSettings cascadeShape = spawnRequest.cascade.useCascadeShape
            ? CloneShape(spawnRequest.cascade.cascadeShape)
            : CloneShape(spawnRequest.shape);

        ApplyCascadeStep(cascadeShape, spawnRequest.cascade);
        DrawShapeGizmo(cascadeShape, position, rotation, cascadePreviewColor);
    }

    private static void DrawShapeGizmo(TelegraphShapeSettings shape, Vector3 position, Quaternion rotation, Color color)
    {
        if (shape == null)
        {
            return;
        }

        Gizmos.color = color;

        switch (shape.shapeType)
        {
            case TelegraphShapeType.Circle:
                Gizmos.DrawWireSphere(position, Mathf.Max(0f, shape.circle.radius));
                break;

            case TelegraphShapeType.Square:
                Gizmos.DrawWireCube(position, new Vector3(Mathf.Max(0f, shape.square.sideLength), 1f, Mathf.Max(0f, shape.square.sideLength)));
                break;

            case TelegraphShapeType.Donut:
                float inner = Mathf.Max(0f, shape.donut.innerRadius);
                float outer = Mathf.Max(inner, shape.donut.outerRadius);
                Gizmos.DrawWireSphere(position, outer);
                Gizmos.DrawWireSphere(position, inner);
                break;

            case TelegraphShapeType.Line:
                float length = Mathf.Max(0f, shape.line.length);
                float width = Mathf.Max(0f, shape.line.width);
                Matrix4x4 oldLineMatrix = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(length, 1f, width));
                Gizmos.matrix = oldLineMatrix;
                break;

            case TelegraphShapeType.Cone:
                DrawConeGizmo(position, rotation, Mathf.Max(0f, shape.cone.radius), Mathf.Clamp(shape.cone.angle, 0f, 360f));
                break;
        }
    }

    private static void DrawConeGizmo(Vector3 position, Quaternion rotation, float radius, float angle)
    {
        Vector3 forward = rotation * Vector3.forward;
        Vector3 rightBoundary = Quaternion.AngleAxis(angle * 0.5f, Vector3.up) * forward * radius;
        Vector3 leftBoundary = Quaternion.AngleAxis(-angle * 0.5f, Vector3.up) * forward * radius;

        Gizmos.DrawLine(position, position + rightBoundary);
        Gizmos.DrawLine(position, position + leftBoundary);
        Gizmos.DrawLine(position, position + forward * radius * (2f / 3f));
        Gizmos.DrawWireSphere(position, radius);
    }

    private static void DrawResizeResultGizmo(
        TelegraphShapeSettings shape,
        TelegraphSizeChangeSettings sizeChange,
        Vector3 position,
        Quaternion rotation,
        Color color)
    {
        if (shape == null || sizeChange == null || sizeChange.mode == SizeChangeMode.None)
        {
            return;
        }

        if (shape.shapeType == TelegraphShapeType.Donut)
        {
            return;
        }

        TelegraphShapeSettings resizedShape = CloneShape(shape);
        ApplyResizeResult(resizedShape, sizeChange);
        DrawShapeGizmo(resizedShape, position, rotation, color);
    }

    private static void ApplyResizeResult(TelegraphShapeSettings shape, TelegraphSizeChangeSettings sizeChange)
    {
        if (shape == null || sizeChange == null)
        {
            return;
        }

        switch (shape.shapeType)
        {
            case TelegraphShapeType.Circle:
                shape.circle.radius = Mathf.Max(0f, sizeChange.maxPrimaryValue);
                break;

            case TelegraphShapeType.Square:
                shape.square.sideLength = Mathf.Max(0f, sizeChange.maxPrimaryValue);
                break;

            case TelegraphShapeType.Line:
                shape.line.length = Mathf.Max(0f, sizeChange.maxPrimaryValue);
                shape.line.width = Mathf.Max(0f, sizeChange.maxSecondaryValue);
                break;

            case TelegraphShapeType.Cone:
                shape.cone.radius = Mathf.Max(0f, sizeChange.maxPrimaryValue);
                shape.cone.angle = Mathf.Clamp(sizeChange.maxSecondaryValue, 0f, 360f);
                break;
        }
    }

    private static TelegraphSpawnRequest CloneRequest(TelegraphSpawnRequest source)
    {
        TelegraphSpawnRequest clone = new TelegraphSpawnRequest();
        if (source == null)
        {
            return clone;
        }

        clone.origin = source.origin;
        clone.worldPosition = source.worldPosition;
        clone.duration = source.duration;
        clone.damage = source.damage;

        CopyShape(source.shape, clone.shape);
        CopySizeChange(source.sizeChange, clone.sizeChange);

        if (source.cascade != null)
        {
            clone.cascade.cascades = source.cascade.cascades;
            clone.cascade.cascadeDelay = source.cascade.cascadeDelay;
            clone.cascade.cascadeCount = source.cascade.cascadeCount;
            clone.cascade.useCascadeShape = source.cascade.useCascadeShape;
            CopyShape(source.cascade.cascadeShape, clone.cascade.cascadeShape);
            clone.cascade.circleRadiusStep = source.cascade.circleRadiusStep;
            clone.cascade.squareSideLengthStep = source.cascade.squareSideLengthStep;
            clone.cascade.donutInnerRadiusStep = source.cascade.donutInnerRadiusStep;
            clone.cascade.donutOuterRadiusStep = source.cascade.donutOuterRadiusStep;
            clone.cascade.lineLengthStep = source.cascade.lineLengthStep;
            clone.cascade.lineWidthStep = source.cascade.lineWidthStep;
            clone.cascade.coneRadiusStep = source.cascade.coneRadiusStep;
            clone.cascade.coneAngleStep = source.cascade.coneAngleStep;
        }

        return clone;
    }

    private static TelegraphShapeSettings CloneShape(TelegraphShapeSettings source)
    {
        TelegraphShapeSettings clone = new TelegraphShapeSettings();
        CopyShape(source, clone);
        return clone;
    }

    private static void CopyShape(TelegraphShapeSettings source, TelegraphShapeSettings destination)
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

    private static void CopySizeChange(TelegraphSizeChangeSettings source, TelegraphSizeChangeSettings destination)
    {
        if (source == null || destination == null)
        {
            return;
        }

        destination.mode = source.mode;
        destination.sizeChangeSpeed = source.sizeChangeSpeed;
        destination.maxPrimaryValue = source.maxPrimaryValue;
        destination.maxSecondaryValue = source.maxSecondaryValue;
        destination.stepCount = source.stepCount;
        destination.timeBetweenSteps = source.timeBetweenSteps;
        destination.lastMomentDelay = source.lastMomentDelay;
    }

    private static void ApplyCascadeStep(TelegraphShapeSettings shape, TelegraphCascadeSettings cascade)
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
