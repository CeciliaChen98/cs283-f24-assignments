using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W)) MoveForward(moveSpeed);
        else if (Input.GetKey(KeyCode.S)) MoveForward(-moveSpeed);
        else if (Input.GetKey(KeyCode.D)) MoveRight(moveSpeed);
        else if (Input.GetKey(KeyCode.A)) MoveRight(-moveSpeed);

    }

    void MoveForward(float speed)
    {
        transform.Translate(transform.forward* speed);
    }

    void MoveRight(float speed)
    {
        transform.Translate(transform.right * speed);
    }
}
