using System.Collections;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private float moveX;
    private float moveY;
    private float setMoveY;
    private Vector3 moveDirection;
    private String lastInputAxis = "";
    private float lastInput = 0f;
    private float movement;

    private void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

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

        movement *= Time.deltaTime;

        transform.Translate(0, 0, movement);
    }

    IEnumerator Rotate(float angle)
    {
        //Debug.Log("rotate");
        //hasRotated = true;
        transform.RotateAround(transform.position, transform.up, angle);
        yield return null;
    }
}


