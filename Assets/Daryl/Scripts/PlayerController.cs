﻿using System.Collections;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = .5f;

    private String lastInputAxis = "Vertical";
    private float lastInput = 1f;
    private float movement;

    PlankRotationManager plankRotationManager;
    PlayerTransitionPlanks playerTransitionPlanks;

    //PlayerPlankDetection playerPlankDetection;

    private void Start()
    {
        plankRotationManager = GameObject.Find("PlankManager").GetComponent<PlankRotationManager>();
        playerTransitionPlanks = GetComponent<PlayerTransitionPlanks>();
    }

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (plankRotationManager.isRotating || playerTransitionPlanks.isRotating) return;

        if (verticalInput != 0)
        {
            if (this.lastInputAxis.Equals("Vertical") && Math.Sign(this.lastInput) != Math.Sign(verticalInput))
            {
                StartCoroutine(Rotate(180f));
            }
            this.lastInputAxis = "Vertical";
            this.lastInput = verticalInput;
        }

        if (horizontalInput != 0)
        {
            if (this.lastInputAxis.Equals("Horizontal") && Math.Sign(this.lastInput) != Math.Sign(horizontalInput))
            {
                StartCoroutine(Rotate(90f));
            }
            this.lastInputAxis = "Horizontal";
            this.lastInput = horizontalInput;
        }

        if (verticalInput != 0 || horizontalInput != 0) movement = 1;
        else if (verticalInput == 0 && horizontalInput == 0) movement = 0;

        movement *= Time.deltaTime * moveSpeed;

        transform.Translate(0, 0, movement);

        /*
        transform plank = playerPlankDetection.currentPlank.transform;
        switch (Math.Sign(verticalInput))
        {
        case 0:
            break;
        case 1:
            transform.forward = plank.up;
            Translate()
            break;
        case -1:
            transform.forward = -plank.up;
            Translate()
            break;
        }
        switch (Math.Sign(horizontalInput))
        {
        case 0:
            break;
        case 1:
            transform.forward = plank.right;
            Translate()
            break;
        case -1:
            transform.forward = -plank.right;
            Translate()
            break;
        }
        */
    }

    IEnumerator Rotate(float angle)
    {
        //Debug.Log("rotate");
        //hasRotated = true;
        transform.RotateAround(transform.position, transform.up, angle);
        yield return null;
    }

    private void Translate()
    {
        transform.Translate(transform.InverseTransformDirection(transform.forward) * 0.5f * Time.deltaTime);
    }
}


