using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /* 
    private float speed = 5f;
    Rigidbody rigid;

    private void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
    }


    private void Move()
    {
        rigid.velocity = new Vector3(Input.GetAxis("Horizontal")*speed, rigid.velocity.y, Input.GetAxis("Vertical")*speed);
    }*/

    private float moveX;
    private float moveY;
    private float moveSpeed = 2f;
    private float setMoveY;
    private float rotationSpeed = 200f;

    private Rigidbody rigid;
    private Vector3 moveDirection;


    private void Start()
    {
        rigid = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Rotate();
    }


    // Moves player
    private void Move()
    {
        // Gets horizontal input and vertical input
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        
        // Sets vertical direction after key down
        if (moveY != 0){
            setMoveY = moveY;

            // Defines a direction to move player based on vertical input
            moveDirection = new Vector3(0, 0, setMoveY).normalized;

            // Moves player towards vertical input direction
            rigid.MovePosition(transform.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
        }
            
        if(moveY == 0){
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }
    }


    // Rotates player
    private void Rotate()
    {
        // If input is to the right
        if (moveX > 0)

            // ...rotate player towards the right
            transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);

        // Else if input is to the left
        else if (moveX < 0)

            // ...rotate player towards the left
            transform.RotateAround(transform.position, -transform.up, rotationSpeed * Time.deltaTime);
    }



    private void OnCollisionEnter(Collision coll)
    {
        

        if (coll.transform.tag != "Plank")
        {
            rigid.constraints = RigidbodyConstraints.FreezePosition;
            
            Debug.Log("bui");
        }
    }


}





