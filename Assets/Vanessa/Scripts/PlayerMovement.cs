using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveX;
    private float moveY;
    private float moveYStick;
    private float setMoveY;

    [SerializeField] GameObject parent;

    public float moveSpeed;
    public float rotationSpeed;

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

    
    private void Move()
    {
        // Gets horizontal input and vertical input
        if (Input.GetAxis("Vertical")!=0)
        {
            rigid.velocity = (parent.transform.position - this.transform.position) * Input.GetAxis("Vertical") * moveSpeed ;
        }
        else if (Input.GetAxis("Vertical")==0)
        {
            rigid.velocity = new Vector3 (0,0,0);
        }


        /*moveY = Input.GetAxis("Vertical");
        
        //moveYStick = Input.GetAxis("Y-Axis");
        
        // Sets vertical direction after key down
        if (moveY >= 0){
           // setMoveY = moveY;

            // Defines a direction to move player based on vertical input
            moveDirection = new Vector3(0, 0, setMoveY).normalized;

            // Moves player towards vertical input direction
            //rigid.MovePosition(transform.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
        }
        
        if(moveY == 0){
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        }*/
    }

    
    // Rotates player
    private void Rotate()
    {
        moveX = Input.GetAxis("Horizontal") ;
        // If input is to the right
        if (moveX > 0)
        {
            // ...rotate player towards the right
            transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
        }
        // Else if input is to the left
        else if (moveX < 0)
        {
            // ...rotate player towards the left
            transform.RotateAround(transform.position, -transform.up, rotationSpeed * Time.deltaTime);
        }
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





