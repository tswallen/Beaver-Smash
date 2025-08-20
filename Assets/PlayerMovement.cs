//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMovement : MonoBehaviour
//{
//    private float playerSpeed = 5.0f;
//    private float jumpHeight = 1.5f;
//    private float gravityValue = -9.81f;

//    private CharacterController controller;
//    private Vector3 playerVelocity;
//    private bool groundedPlayer;

//    [Header("Input Actions")]
//    public InputActionReference moveAction; // expects Vector2
//    public InputActionReference jumpAction; // expects Button

//    private void Awake()
//    {
//        controller = gameObject.AddComponent<CharacterController>();
//    }

//    private void OnEnable()
//    {
//        moveAction.action.Enable();
//        jumpAction.action.Enable();
//    }

//    private void OnDisable()
//    {
//        moveAction.action.Disable();
//        jumpAction.action.Disable();
//    }

//    void Update()
//    {
//        groundedPlayer = controller.isGrounded;
//        if (groundedPlayer && playerVelocity.y < 0)
//        {
//            playerVelocity.y = 0f;
//        }

//        // Read input
//        Vector2 input = moveAction.action.ReadValue<Vector2>();
//        Vector3 move = new Vector3(input.x, 0, input.y);
//        move = Vector3.ClampMagnitude(move, 1f);

//        if (move != Vector3.zero)
//        {
//            transform.forward = move;
//        }

//        // Jump
//        if (jumpAction.action.triggered && groundedPlayer)
//        {
//            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
//        }

//        // Apply gravity
//        playerVelocity.y += gravityValue * Time.deltaTime;

//        // Combine horizontal and vertical movement
//        Vector3 finalMove = (move * playerSpeed) + (playerVelocity.y * Vector3.up);
//        controller.Move(finalMove * Time.deltaTime);
//    }
//}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float playerSpeed = 5.0f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    [Header("Input Actions")]
    public InputActionReference moveAction; // expects Vector2 for WASD movement
    public InputActionReference jumpAction; // expects Button for jump

    [Header("References")]
    public Transform gunTransform; // assign the child cube in Inspector
    public Camera mainCamera;       // assign your main Camera in Inspector

    private void Awake()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        jumpAction.action.Disable();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Read WASD input for movement (no rotation of parent)
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        Vector3 move = new Vector3(input.y, 0, -input.x); //change axis
        move = Vector3.ClampMagnitude(move, 1f);

        // Movement vector (parent moves, but does NOT rotate)
        Vector3 finalMove = move * playerSpeed;

        // Handle jump
        if (jumpAction.action.triggered && groundedPlayer)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        // Apply gravity
        playerVelocity.y += gravityValue * Time.deltaTime;
        finalMove += playerVelocity.y * Vector3.up;

        // Move the character controller
        controller.Move(finalMove * Time.deltaTime);

        // Rotate child cube to face the mouse cursor position in world

        if (gunTransform != null && mainCamera != null)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // y=0 plane

            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 hitPoint = ray.GetPoint(enter);

                Vector3 direction = hitPoint - gunTransform.position;
                direction.y = 0; // keep only horizontal direction

                if (direction.sqrMagnitude > 0.001f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    gunTransform.rotation = targetRotation;
                }
            }
        }
    }
}
