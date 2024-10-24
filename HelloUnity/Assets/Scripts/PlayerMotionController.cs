using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotionController : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public float turnSpeed = 30.0f;
    public GameObject charac; 
    private Animator animator;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        animator = charac.GetComponent<Animator>();
        controller = charac.GetComponent<CharacterController>();
        
        if (animator == null)
        {
            Debug.Log("Hello Unity");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            MoveForward(moveSpeed);
            animator.SetFloat("Speed", moveSpeed);
            animator.SetFloat("Turn", 0.0f);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            MoveForward(-moveSpeed);
            animator.SetFloat("Speed", moveSpeed);
            animator.SetFloat("Turn", 0.0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            TurnRight(turnSpeed);
            animator.SetFloat("Speed", 0.0f);
            animator.SetFloat("Turn", -1.0f);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            TurnRight(-turnSpeed);
            animator.SetFloat("Speed", 0.0f);
            animator.SetFloat("Turn", 1.0f);
        }
        else
        {
            animator.SetFloat("Speed", 0.0f);
            animator.SetFloat("Turn", 0.0f);
        }

       }

        void MoveForward(float speed)
    {
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
        controller.Move(transform.forward * speed * Time.deltaTime);
        transform.position = controller.transform.position;
    }

    void TurnRight(float degree)
    {
        transform.Rotate(Vector3.up, degree * Time.deltaTime);
    }
}
