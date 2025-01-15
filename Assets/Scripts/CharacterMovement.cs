using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    private const string VERTICAL = "Vertical";
    private const string HORIZONTAL = "Horizontal";

    public float speed = 5f;
    public float jumpForce = 100.0f;
    public CharacterController controller;

    private float verticalVelocity;
    private Vector2 movement;
    private bool isJumping;

    void Start() {
        isJumping = false;
    }    

    void Update() {
        float verticalInput = Input.GetAxisRaw(VERTICAL);
        float horizontalInput = Input.GetAxisRaw(HORIZONTAL);

        if (Input.GetButtonDown("Jump")) {
            isJumping = true;
        } else {
            isJumping = false;
        }

        movement = new Vector2(horizontalInput, verticalInput).normalized;
    }

    void FixedUpdate() {
        CalculateGravity();
        Vector3 move = CalculateMovement();

        if (isJumping && controller.isGrounded) {
            Jump();
        }

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
