using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float turnSpeed = 15.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) MoveForward(moveSpeed);
        else if (Input.GetKey(KeyCode.S)) MoveForward(-moveSpeed);
        if (Input.GetKey(KeyCode.D)) TurnRight(turnSpeed);
        else if (Input.GetKey(KeyCode.A)) TurnRight(-turnSpeed);

    }

    void MoveForward(float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void TurnRight(float degree)
    {
        transform.Rotate(0, degree * Time.deltaTime, 0);
    }
}
