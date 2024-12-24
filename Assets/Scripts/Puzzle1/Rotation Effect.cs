using UnityEngine;

public class RotationEffect : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f; // Degrees per second

    private void Update()
    {
        // Rotate around the Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}