using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransitionPlanks : MonoBehaviour
{
    [SerializeField] string transitionPointName = "TransitionPoint";

    DPlayerController playerController;
    PlayerPlankDetection playerPlankDetection;
    Rigidbody rigid;

    private bool isRotating = false;

    private float maxRotation = 90f;
    private float rotationSpeed = 90f;
    private float objectAngle = 0f;
    private float targetRotation = 0f;
    private float gravity = 10;
    private Vector3 myNormal;

    private void Start()
    {
        playerController = GetComponent<DPlayerController>();
        playerPlankDetection = GetComponent<PlayerPlankDetection>();
        rigid = GetComponent<Rigidbody>();
        myNormal = transform.up;
        rigid.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        if (playerPlankDetection.currentPlank)
        {
            rigid.AddForce(-gravity * rigid.mass * playerPlankDetection.currentPlank.forward);

        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!playerPlankDetection.currentPlank)
        {
            return;
        }

        if (collider.name.Equals(transitionPointName) && !isRotating)
        {
            if (playerPlankDetection.currentPlank.GetComponent<DPlankRotation>().activePivot.name.Equals("Pivot R"))
            {
                if (playerPlankDetection.currentPlank.GetComponent<DPlankRotation>().canRotateClockwiseR &&
                    playerPlankDetection.currentPlank.GetComponent<DPlankRotation>().canRotateCounterclockwiseR)
                {
                    return;
                }

                if (playerPlankDetection.currentPlank.GetComponent<DPlankRotation>().canRotateClockwiseR)
                {
                    StartCoroutine(RotatePlayer(1));
                    //Debug.Log("R coroutine1");
                }

                if (playerPlankDetection.currentPlank.GetComponent<DPlankRotation>().canRotateCounterclockwiseR)
                {
                    StartCoroutine(RotatePlayer(-1));
                    //Debug.Log("R coroutine2");
                }
            }

            if (playerPlankDetection.currentPlank.GetComponent<DPlankRotation>().activePivot.name.Equals("Pivot L"))
            {
                if (playerPlankDetection.currentPlank.GetComponent<DPlankRotation>().canRotateClockwiseL &&
                    playerPlankDetection.currentPlank.GetComponent<DPlankRotation>().canRotateCounterclockwiseL)
                {
                    return;
                }

                if (playerPlankDetection.currentPlank.GetComponent<DPlankRotation>().canRotateClockwiseL)
                {
                    StartCoroutine(RotatePlayer(-1));
                    //Debug.Log("L coroutine1");
                }

                if (playerPlankDetection.currentPlank.GetComponent<DPlankRotation>().canRotateCounterclockwiseL)
                {
                    StartCoroutine(RotatePlayer(1));
                    //Debug.Log("L coroutine2");
                }
            }
        }
    }

    IEnumerator RotatePlayer(int direction)
    {
        objectAngle = 0f;

        this.isRotating = true;

        float currentAngle = transform.eulerAngles.x;

        float targetAngle = currentAngle + 90 * direction;

        // While the Plank has not reached max rotation
        while (objectAngle < maxRotation)
        {
            // Increase targetRotation by rotationSpeed
            // Round to integer to prevent non-integer angles from deltaTime 
            targetRotation = Mathf.RoundToInt(rotationSpeed * Time.deltaTime);

            // Increase objectAngle by targetRotation
            objectAngle += targetRotation;

            // Rotate plank around given pivot in given direction
            //transform.RotateAround(transform.position, playerPlankDetection.currentPlank.right * direction, targetRotation);

            transform.Rotate(targetRotation * direction, 0, 0);

            //transform.position += transform.forward * Time.deltaTime * .01f;

            playerController.moveSpeed = 0f;

            // Returns to top of while loop
            yield return null;
        }

        Invoke("ResetRotation", 2f);
    }

    private void ResetRotation()
    {
        this.isRotating = false;
    }
}
