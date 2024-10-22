using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldPointFollowCamera : MonoBehaviour
{
    public Transform cameraTransform;  // Drag the camera here in the Inspector
    public Vector3 holdPointOffset = new Vector3(0, 0, 1);  // Adjust the offset as needed

    void LateUpdate()
    {
        // Follow the camera's position but with an offset
        transform.position = cameraTransform.position + cameraTransform.forward * holdPointOffset.z +
                             cameraTransform.up * holdPointOffset.y + 
                             cameraTransform.right * holdPointOffset.x;

        // Match the camera's rotation
        transform.rotation = cameraTransform.rotation;
    }
}
