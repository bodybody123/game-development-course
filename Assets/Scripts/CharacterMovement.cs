using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const string VERTICAL = "Vertical";
    private const string HORIZONTAL = "Horizontal";
    private const string JUMP = "Jump";

    public float speed = 5f;
    public float rotationSmooth = 3f;
    public float jumpForce = 100.0f;
    public CharacterController controller;

    private float verticalInput;
    private float horizontalInput;

    private float verticalVelocity;
    private Vector2 movement;

    void Update() {
        verticalInput = Input.GetAxisRaw(VERTICAL);
        horizontalInput = Input.GetAxisRaw(HORIZONTAL);

        if (Input.GetButtonDown(JUMP) && controller.isGrounded) {
            Jump();
        }

        movement = new Vector2(horizontalInput, verticalInput).normalized;

        if (movement == Vector2.zero) {
            return;
        }

        FaceDirection();
    }

    void FixedUpdate() {
        CalculateGravity();
        Vector3 move = CalculateMovement();

        controller.Move((move + (Vector3.up * verticalVelocity)) * Time.fixedDeltaTime);
    }

    Vector3 CalculateMovement() {
        Vector3 cameraFwd = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraFwd.y = 0f;
        cameraRight.y = 0f;

        cameraFwd.Normalize();
        cameraRight.Normalize();

        return (cameraFwd * movement.y + cameraRight * movement.x) * speed;
    }

    // Handle PlayerRotation
    private void FaceDirection() {
        // Using quaternion lerp (Linear Interpolation) to smoothly rotate player
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(CalculateMovement()),
            rotationSmooth * Time.deltaTime
        );
    }

    void CalculateGravity() {
        if (verticalVelocity < 0f && controller.isGrounded) {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        } else {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    private void Jump() {
        verticalVelocity += jumpForce;
    }
}
