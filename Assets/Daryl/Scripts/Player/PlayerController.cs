using System.Collections;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;

    [SerializeField] private string lastInputAxis = "Vertical";
    [SerializeField] private float lastInput = 1f;

    private float verticalInput;
    private float horizontalInput;
    private float movement;

    PlankRotationManager plankRotationManager;
    PlayerTransitionPlanks playerTransitionPlanks;
    PlayerPlankDetection playerPlankDetection;

    private void Start()
    {
        playerPlankDetection = GetComponent<PlayerPlankDetection>();
        plankRotationManager = GameObject.Find("PlankManager").GetComponent<PlankRotationManager>();
        playerTransitionPlanks = GetComponent<PlayerTransitionPlanks>();
    }

    private void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (plankRotationManager.isRotating || playerTransitionPlanks.isRotating) return;

        InputMove();
    }

    private void InputMove()
    {
        if (verticalInput != 0)
        {
            // Same axis, different sign
            if (lastInputAxis.Equals("Vertical") && Math.Sign(lastInput) != Math.Sign(verticalInput)) transform.RotateAround(transform.position, transform.up, 180f);

            // Different axis, same sign
            if (lastInputAxis.Equals("Horizontal") && Math.Sign(lastInput) == Math.Sign(verticalInput)) transform.RotateAround(transform.position, transform.up, -90f);

            // Different axis, different sign
            if (lastInputAxis.Equals("Horizontal") && Math.Sign(lastInput) != Math.Sign(verticalInput)) transform.RotateAround(transform.position, transform.up, 90f);

            lastInputAxis = "Vertical";
            lastInput = verticalInput;
        }

        if (horizontalInput != 0)
        {
            // Same axis, different sign
            if (lastInputAxis.Equals("Horizontal") && Math.Sign(lastInput) != Math.Sign(horizontalInput)) transform.RotateAround(transform.position, transform.up, 180f);

            // Different axis, same sign
            if (lastInputAxis.Equals("Vertical") && Math.Sign(lastInput) == Math.Sign(horizontalInput)) transform.RotateAround(transform.position, transform.up, 90f);

            // Different axis, different sign
            if (lastInputAxis.Equals("Vertical") && Math.Sign(lastInput) != Math.Sign(horizontalInput)) transform.RotateAround(transform.position, transform.up, -90f);

            lastInputAxis = "Horizontal";
            lastInput = horizontalInput;
        }

        if (verticalInput != 0 || horizontalInput != 0) movement = 1;
        else if (verticalInput == 0 && horizontalInput == 0) movement = 0;

        movement *= Time.deltaTime * moveSpeed;

        transform.Translate(0, 0, movement);
    }
}


