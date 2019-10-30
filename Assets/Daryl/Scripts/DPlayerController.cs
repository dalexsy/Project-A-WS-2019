using System.Collections;
using UnityEngine;

public class DPlayerController : MonoBehaviour
{
    private float moveX;
    private float moveY;
    private float moveXC;
    private float moveYC;
    private float setMoveY;

    public float moveSpeed = 0f;
    private float maxSpeed = 1f;

    //private float accelerationRate = 1f;
    //private float decelerationRate = 1f;

    private float rotationSpeed = 100f;

    private Rigidbody rigid;
    private Vector3 moveDirection;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Gets horizontal input
        moveX = Mathf.RoundToInt(Input.GetAxis("kHorizontal"));

        // Gets vertical input
        moveY = Mathf.RoundToInt(Input.GetAxis("kVertical"));

        // Gets horizontal input
        moveXC = Mathf.RoundToInt(Input.GetAxis("Horizontal"));

        // Gets vertical input
        moveYC = Mathf.RoundToInt(Input.GetAxis("Vertical"));

        // Sets vertical direction after key down
        if (moveY != 0)
            setMoveY = moveY;

        // Defines a direction to move player based on vertical input
        moveDirection = new Vector3(0, 0, setMoveY).normalized;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();

        if (Input.GetKeyDown("1"))
        {
            transform.Rotate(90, 0, 0);
        }

        if (Input.GetKeyDown("2"))
        {
            transform.Rotate(-90, 0, 0);
        }
    }

    // Moves player
    private void Move()
    {
        if (moveY > 0)
        {
            moveSpeed = maxSpeed;
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }

        else if (moveY < 0)
        {
            moveSpeed = maxSpeed;
            transform.position += transform.forward * -1 * Time.deltaTime * moveSpeed;
        }

        else
        {
            moveSpeed = 0f;
        }
    }

    // Moves player
    private void CMove()
    {
        if (moveYC > 0)
        {
            moveSpeed = maxSpeed;
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }

        else if (moveYC < 0)
        {
            moveSpeed = maxSpeed;
            transform.position += transform.forward * -1 * Time.deltaTime * moveSpeed;
        }

        else
        {
            moveSpeed = 0f;
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

    // Rotates player
    private void RotateC()
    {
        // If input is to the right
        if (moveXC > 0)

            // ...rotate player towards the right
            transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);

        // Else if input is to the left
        else if (moveXC < 0)

            // ...rotate player towards the left
            transform.RotateAround(transform.position, -transform.up, rotationSpeed * Time.deltaTime);
    }
}
