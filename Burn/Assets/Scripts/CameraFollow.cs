using UnityEngine;

/* Allows the camera to follow a target (the target here is the Player)
 */
public class CameraFollow : MonoBehaviour
{

    //Good reference: https://www.youtube.com/watch?v=MFQhpwc6cKE&t=171s

    public Transform target; //get location of the target (set within unity)
    private float followSpeed = .125f; //follow speed of the camera, higher value means faster follow speed
    public Vector3 offset; //location of the camera (set within unity)


    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset; //camera's current position (set within unity) = target position
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, followSpeed);
        transform.position = smoothPosition;
    }

}
