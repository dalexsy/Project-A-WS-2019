using UnityEngine;

public class MPlayerController : MonoBehaviour
{
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


