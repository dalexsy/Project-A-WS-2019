using UnityEngine;

public class DPlayerController : MonoBehaviour
{
    private float moveX;
    private float moveY;
    private float setMoveY;

    private float moveSpeed = 0f;
    private float maxSpeed = 1f;

    //private float accelerationRate = 1f;
    //private float decelerationRate = 1f;

    private float rotationSpeed = 75f;

    private Rigidbody rigid;
    private Vector3 moveDirection;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Gets horizontal input
        moveX = Input.GetAxis("kHorizontal");

        // Gets vertical input
        moveY = Input.GetAxis("kVertical");

        // Sets vertical direction after key down
        if (moveY != 0)
            setMoveY = moveY;

        // Defines a direction to move player based on vertical input
        moveDirection = new Vector3(0, 0, setMoveY).normalized;
    }

    private void FixedUpdate()
    {
        Boost();
        Move();
        Rotate();
    }

    // Moves player
    private void Move()
    {
        Accelerate();
        Decelerate();

        // Moves player towards vertical input direction
        rigid.MovePosition(transform.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
    }

    // Accelerates player
    private void Accelerate()
    {
        // If vertical input
        if (moveY != 0)
        {
            // If movement speed is over maximum speed
            if (moveSpeed >= maxSpeed)

                // ...stop accelerations
                return;

            // ...increase movement speed by acceleration rate
            moveSpeed = maxSpeed;
        }
    }

    // Decelerates player
    private void Decelerate()
    {
        // ...decrease movement speed by deceleration rate
        moveSpeed = 0f;
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

    // Boosts player's speed
    private void Boost()
    {
        // If space bar is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            // ...increase acceleration rate
            //accelerationRate = .02f;

            // ...increase maximum speed
            maxSpeed = 2f;
        }

        else
        {
            // ...reset acceleration rate
            //this.accelerationRate = .01f;

            // ...reset maximum speed
            this.maxSpeed = 1f;
        }
    }
}
