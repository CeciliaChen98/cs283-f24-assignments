using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float speed = 5.0f;          // Movement speed
    public float lookSpeed = 2.0f;      // Mouse look sensitivity
    public float upDownRange = 60.0f;   // Range for up/down rotation
    public float gravity = -9.81f;      // Gravity value

    private CharacterController characterController;
    private Camera playerCamera;
    private float rotationX = 0;
    private Vector3 velocity;
    private GameObject candle;
    private bool canMove = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();

        candle = GameObject.Find("Character/Hand/Candle");
        if (candle != null)
        {
            candle.SetActive(false);
        }
        // Hide the mouse cursor

        Cursor.lockState = CursorLockMode.Locked;
        
        StartCoroutine(DisableMovementForSeconds(4));
    }

    private IEnumerator DisableMovementForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canMove = true;
    }

    void Update()
    {
        if (!canMove) return;

        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? 1.8f*speed : speed;

        float moveDirectionY = Input.GetAxis("Vertical") * currentSpeed;
        float moveDirectionX = Input.GetAxis("Horizontal") * currentSpeed;

        // Create movement vector
        Vector3 move = transform.right * moveDirectionX + transform.forward * moveDirectionY;

        if (characterController.isGrounded){
            velocity.y = 0;
        }else{
            velocity.y += gravity * Time.deltaTime; // Apply gravity when not grounded
        }
        // Move the character controller
        characterController.Move((move + velocity) * Time.deltaTime);

        // Look around
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -upDownRange, upDownRange);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}