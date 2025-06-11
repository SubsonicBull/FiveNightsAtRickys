using UnityEngine;

public class Floating : MonoBehaviour
{
    [Header("Floating Settings")]
    public float floatSpeed = 1f;     // Speed of the up/down motion
    public float floatRange = 0.5f;   // Amplitude of the floating

    [Header("Rotation Settings")]
    public Vector3 rotationAxis = Vector3.up;  // Axis to rotate around
    public float rotationSpeed = 10f;          // Degrees per second

    private Vector3 startPos;

    void Start()
    {
        // Save the initial position
        startPos = transform.position;

        // Normalize the rotation axis to ensure consistent behavior
        rotationAxis.Normalize();
    }

    void Update()
    {
        // Floating motion (sinusoidal)
        float newY = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPos + new Vector3(0f, newY, 0f);

        // Rotation motion
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime, Space.Self);
    }
}
