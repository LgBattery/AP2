using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController controller;

    public float speed = 12f;
    public float gravity = -19.62f;
    public float jumpHeight = 3;
    //double jump
    public int counter = 1;

    public KeyCode sprint;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform cocaineCheck;
    public float cocaineDistance = 0.4f;
    public LayerMask cocaineMask;
    public Transform teleportTarget;
    public GameObject Player;

    Vector3 velocity;
    bool isGrounded;
    bool isOnCocaine;


    // Update is called once per frame
    void Update()
    {
        if (Physics.CheckSphere(groundCheck.position, groundDistance, groundMask))
        {
            isGrounded = true;
            print("munayna");
        }
        
        else
        {
            isGrounded = false;
        }

        if (Physics.CheckSphere(cocaineCheck.position, cocaineDistance, cocaineMask))
        {
            isOnCocaine = true;
            print("munayna");
        }

        else
        {
            isOnCocaine = false;
        }



        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && counter <2)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            counter ++;
        }
        
        if(isGrounded)
        {
            counter = 1;
        }

        if(Input.GetKeyDown(sprint))
        {
            speed = speed * 1.5f;
        }

        if(Input.GetKeyUp(sprint))
        {
            speed = 12;
        }
        
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if(isOnCocaine)
        {
            Player.transform.position = teleportTarget.transform.position;
        }
    }
}
