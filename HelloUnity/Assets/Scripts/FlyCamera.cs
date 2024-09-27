using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    // Variables for camera movement and rotation
    public float moveSpeed = 0.1f;  // Speed of camera movement
    public float rotateSpeed = 2.0f;   // Speed of mouse rotation

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotateSpeed;
        float mouseY = - Input.GetAxis("Mouse Y") * rotateSpeed;

        Quaternion rotation = transform.rotation;
        Quaternion horiz = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion vert = Quaternion.AngleAxis(mouseY, Vector3.right);
        transform.rotation = horiz * rotation * vert;

        if (Input.GetKey(KeyCode.W)) MoveForward(moveSpeed);
        else if (Input.GetKey(KeyCode.S)) MoveForward(-moveSpeed);
        else if (Input.GetKey(KeyCode.D)) MoveRight(moveSpeed);
        else if (Input.GetKey(KeyCode.A)) MoveRight(-moveSpeed);
    }

    void MoveForward(float speed)
    {
        transform.Translate(0, 0, speed);
    }

    void MoveRight(float speed)
    {
        transform.Translate(speed, 0, 0);
    }
}
