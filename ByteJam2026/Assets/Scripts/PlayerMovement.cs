using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    [SerializeField]
    private float movementSpeed = 5f;
    private CharacterController controller;
    private Animator animator;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private float jumpHeight = 2f;
    [SerializeField]
    private float gravity = -9.81f;
    private float verticalVelocity;

    private void Awake() {
        controller = GetComponent <CharacterController >();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update(){
        float verticalInput = 0f;
        float horizontalInput = 0f;
        if (Keyboard.current.wKey.isPressed)
        {
            verticalInput = 1f;
        }
        else if (Keyboard.current.sKey.isPressed)
        {
            verticalInput = -1f;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            horizontalInput = -1f;
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            horizontalInput = 1f;
        }

        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 movement =
            cameraForward * verticalInput +
            cameraRight * horizontalInput;

        if (movement.sqrMagnitude > 1f)
        {
            movement.Normalize();
        }

        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }

        movement.x *= movementSpeed;
        movement.z *= movementSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame && controller.isGrounded)
        {
            verticalVelocity =
                Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;
        movement.y = verticalVelocity;
        controller.Move(movement * Time.deltaTime);

        animator.SetBool(
            "IsWalking",
            horizontalInput != 0f || verticalInput != 0f
        );

        animator.SetBool(
            "IsJumping",
            !controller.isGrounded
        );
    }
}