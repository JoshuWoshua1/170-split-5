using UnityEngine;

public class UFOMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform[] waypointTargets;
    [SerializeField] private float[] waypointDelays;

    [Header("Rotation")]
    [SerializeField] private float rotationSpeed = 45f;
    [SerializeField] private bool rotateSprite = true;

    [Header("Optional")]
    [SerializeField] private float lifetime = 0f;

    private float elapsedTime;
    private int currentWaypointIndex;
    private float nextWaypointTime;

    private void Start()
    {
        currentWaypointIndex = 0;
        nextWaypointTime = waypointDelays != null && waypointDelays.Length > 0 ? waypointDelays[0] : 0f;
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        MoveAlongWaypoints();

        RotateUFO();

        if (lifetime > 0f && elapsedTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void MoveAlongWaypoints()
    {
        if (waypointTargets == null || waypointTargets.Length == 0)
        {
            return;
        }

        if (elapsedTime < nextWaypointTime)
        {
            return;
        }

        if (currentWaypointIndex >= waypointTargets.Length)
        {
            return;
        }

        Transform target = waypointTargets[currentWaypointIndex];
        if (target == null)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex < waypointTargets.Length)
            {
                nextWaypointTime = elapsedTime + (waypointDelays.Length > currentWaypointIndex ? waypointDelays[currentWaypointIndex] : 0f);
            }
            return;
        }

        Vector3 targetPosition = target.position;
        Vector3 direction = (targetPosition - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex < waypointTargets.Length)
            {
                nextWaypointTime = elapsedTime + (waypointDelays.Length > currentWaypointIndex ? waypointDelays[currentWaypointIndex] : 0f);
            }
        }
    }

    private void RotateUFO()
    {
        if (!rotateSprite)
        {
            return;
        }

        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }
}
