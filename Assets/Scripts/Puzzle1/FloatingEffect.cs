using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [SerializeField] private float floatAmplitude = 0.5f; // How high the sphere floats
    [SerializeField] private float floatFrequency = 1f; // How fast the sphere floats up and down

    private Vector3 startPos;

    private void Start()
    {
        // Store the initial position
        startPos = transform.position;
    }

    private void Update()
    {
        // Calculate new position based on a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}

