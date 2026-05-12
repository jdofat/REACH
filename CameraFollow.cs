using UnityEngine;

public class camerafollow : MonoBehaviour
{
     public Transform target;     // The player
    public Vector3 offset = new Vector3(0, 2, -4);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        // Where we want the camera to be
        Vector3 desiredPosition = target.position + offset;

        // Smooth follow (instead of snapping instantly)
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        // Make the camera look at the player
        transform.LookAt(target);
    }

}
