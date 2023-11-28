using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour {
    public int mouseSensitivity = 600;
    public int movementSpeed = 10;
    public Transform cameraTransform;
    public float jumpHeight = 5;
    public AudioSource walkingAudio;

    private float moveVertical = 0;
    private float moveHorizontal = 0;
    private float lookVertical = 0;
    private float lookHorizontal = 0;
    private Vector3 playerInput;
    private Vector3 moveDirection;
    private CharacterController controller;
    private float yVelocity;

    // Start is called before the first frame update
    void Start() {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        // Vector3 vector = transform.forward * -moveHorizontal + transform.right * moveVertical;
        // vector = vector.normalized * movementSpeed;
        playerInput = new Vector3(moveHorizontal, 0, moveVertical).normalized;
        if(yVelocity <= 0 && controller.isGrounded) {
            yVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else {
            yVelocity += Physics.gravity.y * Time.deltaTime;
        }

        Look();
        ManipulateController();

        if(moveVertical == 0 && moveHorizontal == 0) {
            walkingAudio.Stop();
        }
        else {
            if(!walkingAudio.isPlaying) {
                walkingAudio.Play();
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context) {
        var v = context.ReadValue<Vector2>();
        moveVertical = v.y / 10;
        moveHorizontal = v.x / 10;
    }

    public void OnLook(InputAction.CallbackContext context) {
        var v = context.ReadValue<Vector2>();
        lookVertical = v.y * ((float)mouseSensitivity / 100f);
        lookHorizontal = v.x * ((float)mouseSensitivity / 100f);
    }

    public void OnJump(InputAction.CallbackContext context) {
        if(context.phase == InputActionPhase.Started && controller.isGrounded) {
            // lastJump = 0f;
            // rb.velocity += new Vector3(0, 40, 0);
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
    }

    private void Look() {
        transform.Rotate(Vector3.up * lookHorizontal);
        cameraTransform.Rotate(Vector3.right * -lookVertical);
    }

    void ManipulateController() {
        moveDirection = transform.rotation * playerInput;
        moveDirection *= movementSpeed * 1.3f;
        moveDirection.y = yVelocity;
        moveDirection *= Time.deltaTime;
        controller.Move(moveDirection);
    }
}
