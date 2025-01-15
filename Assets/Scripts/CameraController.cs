using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    private float yaw = 0f;
    private float pitch = 0f;

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X");
        float inputY = Input.GetAxis("Mouse Y");

        yaw += inputX;
        pitch -= inputY;

        pitch = Mathf.Clamp(pitch, -89f, 89f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 offset = rotation * new Vector3(0, 0, -distance);
        Vector3 desiredPosition = target.position + Vector3.up + offset;

        transform.position = desiredPosition;
        transform.LookAt(target.position);
    }
}
