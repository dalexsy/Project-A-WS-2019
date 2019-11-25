using System.Collections;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private string lastInputAxis = "Vertical";
    [SerializeField] private float lastInput = 1f;

    private float movement;

    PlankRotationManager plankRotationManager;
    PlayerManager playerManager;
    PlayerTransitionPlanks playerTransitionPlanks;
    PlayerPlankDetection playerPlankDetection;

    private void Start()
    {
        playerPlankDetection = GetComponent<PlayerPlankDetection>();
        playerManager = GameObject.Find("Player Manager").GetComponent<PlayerManager>();
        plankRotationManager = GameObject.Find("Plank Manager").GetComponent<PlankRotationManager>();
        playerTransitionPlanks = GetComponent<PlayerTransitionPlanks>();
    }

    private void Update()
    {
        if (plankRotationManager.isRotating || playerManager.isRotating) return;
        InputMove();
    }

    private void InputMove()
    {
        if (playerManager.verticalInput != 0)
        {
            // Same axis, different sign
            if (lastInputAxis.Equals("Vertical") && Math.Sign(lastInput) != Math.Sign(playerManager.verticalInput)) transform.RotateAround(transform.position, transform.up, 180f);

            // Different axis, same sign
            if (lastInputAxis.Equals("Horizontal") && Math.Sign(lastInput) == Math.Sign(playerManager.verticalInput)) transform.RotateAround(transform.position, transform.up, -90f);

            // Different axis, different sign
            if (lastInputAxis.Equals("Horizontal") && Math.Sign(lastInput) != Math.Sign(playerManager.verticalInput)) transform.RotateAround(transform.position, transform.up, 90f);

            lastInputAxis = "Vertical";
            lastInput = playerManager.verticalInput;
        }

        if (playerManager.horizontalInput != 0)
        {
            // Same axis, different sign
            if (lastInputAxis.Equals("Horizontal") && Math.Sign(lastInput) != Math.Sign(playerManager.horizontalInput)) transform.RotateAround(transform.position, transform.up, 180f);

            // Different axis, same sign
            if (lastInputAxis.Equals("Vertical") && Math.Sign(lastInput) == Math.Sign(playerManager.horizontalInput)) transform.RotateAround(transform.position, transform.up, 90f);

            // Different axis, different sign
            if (lastInputAxis.Equals("Vertical") && Math.Sign(lastInput) != Math.Sign(playerManager.horizontalInput)) transform.RotateAround(transform.position, transform.up, -90f);

            lastInputAxis = "Horizontal";
            lastInput = playerManager.horizontalInput;
        }

        if (playerManager.verticalInput != 0 || playerManager.horizontalInput != 0) movement = 1;
        else if (playerManager.verticalInput == 0 && playerManager.horizontalInput == 0) movement = 0;

        movement *= Time.deltaTime * playerManager.moveSpeed;

        transform.Translate(0, 0, movement);
    }
}


