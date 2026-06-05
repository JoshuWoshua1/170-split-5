using UnityEngine;

public class spincow : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f; // Rotation speed in degrees per second
    [SerializeField] private float bobbingAmplitude = 0.5f; // Amplitude of the bobbing motion
    [SerializeField] private float bobbingFrequency = 1f; // Frequency of the bobbing motion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the cow around its Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Bobbing motion
        float newY = initialPosition.y + Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
