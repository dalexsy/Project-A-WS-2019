using UnityEngine;

public class DPlayerController : MonoBehaviour
{
    /*
    private float moveX;
    private float moveY;
    private float setMoveY;

    private float moveSpeed = 0f;
    private float minSpeed = .01f;
    private float maxSpeed = 1f;

    //private float accelerationRate = .01f;
    //private float decelerationRate = .02f;

    private float rotationSpeed = 60f;

    private Rigidbody rigid;
    private Vector3 moveDirection;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Gets horizontal input
        moveX = Input.GetAxis("Horizontal");

        // Gets vertical input
        moveY = Input.GetAxis("Vertical");

        // Sets vertical direction after key down
        if (moveY != 0)
            setMoveY = moveY;

        // Defines a direction to move player based on vertical input
        moveDirection = new Vector3(0, 0, setMoveY).normalized;
    }

    private void FixedUpdate()
    {
        //Boost();
        Move();
        Rotate();
    }

    // Moves player
    private void Move()
    {
        //Accelerate();
        //Decelerate();
        ThrottleSpeed();

        // Moves player towards vertical input direction
        rigid.MovePosition(transform.position + transform.TransformDirection(moveDirection) * moveSpeed * Time.deltaTime);
    }

    // Accelerates player
    /*
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
            moveSpeed += accelerationRate;
        }
    }

    // Decelerates player
    
    private void Decelerate()
    {
        // If no vertical input and player is still moving or if movement speed exceeds maximum
        if (moveY == 0 && moveSpeed > minSpeed || moveSpeed > maxSpeed)

            // ...decrease movement speed by deceleration rate
            moveSpeed -= decelerationRate;
    }

    // Keeps player in constant motion to prevent camera flipping
    private void ThrottleSpeed()
    {
        // If movement speed is over minimum speed
        if (moveSpeed < minSpeed)

            // ...set movement speed to minimum speed
            moveSpeed = minSpeed;
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
    /*
    private void Boost()
    {
        // If space bar is pressed
        if (Input.GetKey(KeyCode.Space))
        {
            // ...increase acceleration rate
            accelerationRate = .02f;

            // ...increase maximum speed
            maxSpeed = 2f;
        }

        else
        {
            // ...reset acceleration rate
            this.accelerationRate = .01f;

            // ...reset maximum speed
            this.maxSpeed = 1f;
        }
    }*/
    private float moveX;
    private float moveY;
    private float moveKeyX;
    private float moveYStick;
    private float setMoveY;

    private Rigidbody rigid;
    private Vector3 moveDirection;

    public float moveSpeed;
    public float rotationSpeed;

    [SerializeField] GameObject parent;


    private void Start()
    {

        rigid = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Move();
        Rotate();
        MoveKeyboard();
    }

    private void Move()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            rigid.velocity = (parent.transform.position - this.transform.position) * Input.GetAxis("Vertical") * moveSpeed;
        }
        else if (Input.GetAxis("Vertical") == 0)
        {
            rigid.velocity = new Vector3(0, 0, 0);
        }
    }

    private void MoveKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rigid.velocity = (parent.transform.position - this.transform.position) * moveSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            rigid.velocity = (this.transform.position - parent.transform.position) * moveSpeed;
        }

    }

    private void Rotate()
    {
        moveX = Input.GetAxis("Horizontal");
        if (moveX > 0)
        {
            transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
        }
        else if (moveX < 0)
        {
            transform.RotateAround(transform.position, -transform.up, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(transform.position, transform.up, rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
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


