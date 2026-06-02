using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class TelegraphList
{
    [SerializeField] private TelegraphSpawnRequest spawnRequest = new TelegraphSpawnRequest();
    [SerializeField] private float delayBeforeNextTelegraph;

    public TelegraphSpawnRequest SpawnRequest => spawnRequest;
    public float DelayBeforeNextTelegraph => delayBeforeNextTelegraph;

}

public class TelegraphSystem : MonoBehaviour
{
    [Header("Inspector Sequence")]
    [SerializeField] private TelegraphList[] telegraphs;
    [SerializeField] private bool playSequenceOnStart;

    [Header("Shape Prefabs")]
    [SerializeField] private TelegraphCircle circlePrefab;
    [SerializeField] private TelegraphCone conePrefab;
    [SerializeField] private TelegraphDonut donutPrefab;
    [SerializeField] private TelegraphSquare squarePrefab;
    [SerializeField] private TelegraphLine linePrefab;

    private Coroutine sequenceCoroutine;

    private void Start()
    {
        if (playSequenceOnStart)
        {
            PlayTelegraphSequence();
        }
    }

    public void PlayTelegraphSequence()
    {
        if (sequenceCoroutine != null)
        {
            StopCoroutine(sequenceCoroutine);
        }

        sequenceCoroutine = StartCoroutine(SpawnTelegraphsInOrder());
    }

    private IEnumerator SpawnTelegraphsInOrder()
    {
        if (telegraphs == null || telegraphs.Length == 0)
        {
            yield break;
        }

        foreach (TelegraphList entry in telegraphs)
        {
            if (entry == null || entry.SpawnRequest == null)
            {
                continue;
            }

            SpawnTelegraph(CloneSpawnRequest(entry.SpawnRequest));

            if (entry.DelayBeforeNextTelegraph > 0f)
            {
                yield return new WaitForSeconds(entry.DelayBeforeNextTelegraph);
            }
        }

        sequenceCoroutine = null;
    }

    public Telegraph SpawnCircle(float radius, Vector3 worldPosition, Transform origin = null, float duration = 1f, int damage = 1)
    {
        TelegraphSpawnRequest request = CreateBaseRequest(worldPosition, origin, duration, damage);
        request.shape.shapeType = TelegraphShapeType.Circle;
        request.shape.circle.radius = radius;
        return SpawnTelegraph(request);
    }

    public Telegraph SpawnDonut(float innerRadius, float outerRadius, Vector3 worldPosition, Transform origin = null, float duration = 1f, int damage = 1)
    {
        TelegraphSpawnRequest request = CreateBaseRequest(worldPosition, origin, duration, damage);
        request.shape.shapeType = TelegraphShapeType.Donut;
        request.shape.donut.innerRadius = innerRadius;
        request.shape.donut.outerRadius = outerRadius;
        return SpawnTelegraph(request);
    }

    public Telegraph SpawnCircleToDonutCascade(
        float circleRadius,
        float donutInnerRadius,
        float donutOuterRadius,
        Vector3 worldPosition,
        Transform origin = null,
        float duration = 1f,
        int damage = 1,
        float cascadeDelay = 1f,
        int cascadeCount = 2,
        float donutInnerRadiusStep = 0f,
        float donutOuterRadiusStep = 0f)
    {
        TelegraphSpawnRequest request = CreateBaseRequest(worldPosition, origin, duration, damage);
        request.shape.shapeType = TelegraphShapeType.Circle;
        request.shape.circle.radius = circleRadius;

        request.cascade.cascades = true;
        request.cascade.cascadeDelay = cascadeDelay;
        request.cascade.cascadeCount = Mathf.Max(1, cascadeCount);
        request.cascade.useCascadeShape = true;
        request.cascade.cascadeShape.shapeType = TelegraphShapeType.Donut;
        request.cascade.cascadeShape.donut.innerRadius = donutInnerRadius;
        request.cascade.cascadeShape.donut.outerRadius = donutOuterRadius;
        request.cascade.donutInnerRadiusStep = donutInnerRadiusStep;
        request.cascade.donutOuterRadiusStep = donutOuterRadiusStep;

        return SpawnTelegraph(request);
    }

    public Telegraph SpawnTelegraph(TelegraphSpawnRequest request)
    {
        if (request == null)
        {
            Debug.LogWarning("TelegraphSystem: Spawn request was null.");
            return null;
        }

        Telegraph prefab = GetPrefabForShape(request.shape.shapeType);
        if (prefab == null)
        {
            Debug.LogWarning($"TelegraphSystem: No prefab assigned for shape {request.shape.shapeType}.");
            return null;
        }

        Vector3 position = request.worldPosition;
        if (position == Vector3.zero && request.origin != null)
        {
            position = request.origin.position;
        }

        Quaternion rotation = request.origin != null ? request.origin.rotation : Quaternion.identity;
        Telegraph spawned = Instantiate(prefab, position, rotation);
        spawned.ApplySpawnRequest(request, this);
        return spawned;
    }

    private static TelegraphSpawnRequest CreateBaseRequest(Vector3 worldPosition, Transform origin, float duration, int damage)
    {
        return new TelegraphSpawnRequest
        {
            origin = origin,
            worldPosition = worldPosition,
            duration = duration,
            damage = damage
        };
    }

    private static TelegraphSpawnRequest CloneSpawnRequest(TelegraphSpawnRequest source)
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

        clone.shape.shapeType = source.shape.shapeType;
        clone.shape.circle.radius = source.shape.circle.radius;
        clone.shape.square.sideLength = source.shape.square.sideLength;
        clone.shape.donut.innerRadius = source.shape.donut.innerRadius;
        clone.shape.donut.outerRadius = source.shape.donut.outerRadius;
        clone.shape.line.length = source.shape.line.length;
        clone.shape.line.width = source.shape.line.width;
        clone.shape.cone.radius = source.shape.cone.radius;
        clone.shape.cone.angle = source.shape.cone.angle;

        clone.cascade.cascades = source.cascade.cascades;
        clone.cascade.cascadeDelay = source.cascade.cascadeDelay;
        clone.cascade.cascadeCount = source.cascade.cascadeCount;
        clone.cascade.useCascadeShape = source.cascade.useCascadeShape;
        clone.cascade.cascadeShape.shapeType = source.cascade.cascadeShape.shapeType;
        clone.cascade.cascadeShape.circle.radius = source.cascade.cascadeShape.circle.radius;
        clone.cascade.cascadeShape.square.sideLength = source.cascade.cascadeShape.square.sideLength;
        clone.cascade.cascadeShape.donut.innerRadius = source.cascade.cascadeShape.donut.innerRadius;
        clone.cascade.cascadeShape.donut.outerRadius = source.cascade.cascadeShape.donut.outerRadius;
        clone.cascade.cascadeShape.line.length = source.cascade.cascadeShape.line.length;
        clone.cascade.cascadeShape.line.width = source.cascade.cascadeShape.line.width;
        clone.cascade.cascadeShape.cone.radius = source.cascade.cascadeShape.cone.radius;
        clone.cascade.cascadeShape.cone.angle = source.cascade.cascadeShape.cone.angle;
        clone.cascade.circleRadiusStep = source.cascade.circleRadiusStep;
        clone.cascade.squareSideLengthStep = source.cascade.squareSideLengthStep;
        clone.cascade.donutInnerRadiusStep = source.cascade.donutInnerRadiusStep;
        clone.cascade.donutOuterRadiusStep = source.cascade.donutOuterRadiusStep;
        clone.cascade.lineLengthStep = source.cascade.lineLengthStep;
        clone.cascade.lineWidthStep = source.cascade.lineWidthStep;
        clone.cascade.coneRadiusStep = source.cascade.coneRadiusStep;
        clone.cascade.coneAngleStep = source.cascade.coneAngleStep;

        return clone;
    }

    private Telegraph GetPrefabForShape(TelegraphShapeType shapeType)
    {
        switch (shapeType)
        {
            case TelegraphShapeType.Circle:
                return circlePrefab;
            case TelegraphShapeType.Cone:
                return conePrefab;
            case TelegraphShapeType.Donut:
                return donutPrefab;
            case TelegraphShapeType.Square:
                return squarePrefab;
            case TelegraphShapeType.Line:
                return linePrefab;
            default:
                return null;
        }
    }
}
