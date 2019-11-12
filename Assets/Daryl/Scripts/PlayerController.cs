using System.Collections;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    private float verticalInput;
    private float horizontalInput;
    private String lastInputAxis = "Vertical";
    private float lastInput = 1f;
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
        //SwitchMove();
    }

    private void InputMove()
    {
        if (verticalInput != 0)
        {
            // Same axis, different sign
            if (lastInputAxis.Equals("Vertical") && Math.Sign(lastInput) != Math.Sign(verticalInput)) StartCoroutine(Rotate(180f));

            // Different axis, same sign
            if (lastInputAxis.Equals("Horizontal") && Math.Sign(lastInput) == Math.Sign(verticalInput)) StartCoroutine(Rotate(-90f));

            // Different axis, different sign
            if (lastInputAxis.Equals("Horizontal") && Math.Sign(lastInput) != Math.Sign(verticalInput)) StartCoroutine(Rotate(90f));

            lastInputAxis = "Vertical";
            lastInput = verticalInput;
        }

        if (horizontalInput != 0)
        {
            // Same axis, different sign
            if (lastInputAxis.Equals("Horizontal") && Math.Sign(lastInput) != Math.Sign(horizontalInput)) StartCoroutine(Rotate(180f));

            // Different axis, same sign
            if (lastInputAxis.Equals("Vertical") && Math.Sign(lastInput) == Math.Sign(horizontalInput)) StartCoroutine(Rotate(90f));

            // Different axis, different sign
            if (lastInputAxis.Equals("Vertical") && Math.Sign(lastInput) != Math.Sign(horizontalInput)) StartCoroutine(Rotate(-90f));

            lastInputAxis = "Horizontal";
            lastInput = horizontalInput;
        }

        if (verticalInput != 0 || horizontalInput != 0) movement = 1;
        else if (verticalInput == 0 && horizontalInput == 0) movement = 0;

        movement *= Time.deltaTime * moveSpeed;

        transform.Translate(0, 0, movement);
    }

    private void SwitchMove()
    {
        Transform plank = playerPlankDetection.currentPlank.transform;
        switch (Math.Sign(verticalInput))
        {
            case 0:
                break;
            case 1:
                transform.forward = plank.up;
                Translate();
                break;
            case -1:
                transform.forward = -plank.up;
                Translate();
                break;
        }
        switch (Math.Sign(horizontalInput))
        {
            case 0:
                break;
            case 1:
                transform.forward = plank.right;
                Translate();
                break;
            case -1:
                transform.forward = -plank.right;
                Translate();
                break;
        }

    }

    IEnumerator Rotate(float angle)
    {
        transform.RotateAround(transform.position, transform.up, angle);
        yield return null;
    }

    private void Translate()
    {
        transform.Translate(transform.InverseTransformDirection(transform.forward) * 0.5f * Time.deltaTime);
    }
}


