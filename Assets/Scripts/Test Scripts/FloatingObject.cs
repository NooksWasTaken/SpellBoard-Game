using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Floating")]
    [SerializeField] private float floatHeight = 0.25f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Rotation")]
    [SerializeField] private float yRotationSpeed = 60f;
    [SerializeField] private float zRotationSpeed = 20f;

    private Vector3 startPosition;
    private Vector3 rotation;

    private void Awake()
    {
        startPosition = transform.localPosition;
        rotation = transform.localEulerAngles;
    }

    private void Update()
    {
        // float
        Vector3 position = startPosition;
        position.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.localPosition = position;

        // rotate
        rotation.y += yRotationSpeed * Time.deltaTime;
        rotation.z += zRotationSpeed * Time.deltaTime;

        transform.localEulerAngles = rotation;
    }
}