using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VPlankRotation : MonoBehaviour
{
    [SerializeField] float maxRotation = 90f;
    [SerializeField] float rotationSpeed = 30f;

    [SerializeField] GameObject pulseParticlePrefab = null;

    public Transform activePivot = null;

    public bool canRotateClockwise = true;
    public bool canRotateCounterclockwise = true;

    private bool isRotating = false;

    private CollisionDetection collisionDetection;
    private PlankCollisionDetection plankCollisionDetection;
    private PlankConnection plankConnection;
    private VPlayerMovement player;

    private float objectAngle = 0f;
    private float targetRotation = 0f;


    private GameObject mainCamera;

    private IEnumerator connectionCoroutine;

    private void Start()
    {
        player = GetComponent<VPlayerMovement>();
        mainCamera = GameObject.Find("Main Camera");
        collisionDetection = GetComponent<CollisionDetection>();
        plankCollisionDetection = GetComponentInChildren<PlankCollisionDetection>();
        plankConnection = GetComponent<PlankConnection>();
    }

    private void Update()
    {
        if (collisionDetection.isCollidingWithTarget == false)
            KeyboardRotationInput();
    }

    private void KeyboardRotationInput()
    {
        if (!activePivot)
            return;


        if (!isRotating)
        {
            if (canRotateClockwise)
            {
                if (Input.GetKeyDown("e"))
                {
                    StartCoroutine(RotatePlank(-1, activePivot));
                }
                if (Input.GetKey(KeyCode.Joystick1Button6))
                {
                    StartCoroutine(RotatePlank(-1, activePivot));
                }
            }

            if (canRotateCounterclockwise)
            {
                if (Input.GetKeyDown("q"))
                {
                    StartCoroutine(RotatePlank(1, activePivot));
                }
                if (Input.GetKey(KeyCode.Joystick1Button7))
                {
                    StartCoroutine(RotatePlank(1, activePivot));
                }
            }
        }
        if (!activePivot)
            return;
    }


    // Rotates Plank
    // Requires direction (1 for down, -1 for up) and pivot (lPivot, rPivot)
    IEnumerator RotatePlank(int direction, Transform pivot)
    {
        // Starts coroutine to connect planks using active pivot
        plankConnection.ConnectPlanks(pivot);

        // Reset object angle
        objectAngle = 0f;

        // Set isRotating to true to prevent multiple rotations
        this.isRotating = true;

        // Create visual feedback on pivot to be rotated from
        GameObject pulse = Instantiate(pulseParticlePrefab, pivot.transform.position, pulseParticlePrefab.transform.rotation);

        // Destroy particle system once system has run once
        Destroy(pulse, pulse.GetComponent<ParticleSystem>().main.startLifetimeMultiplier);

        // While the Plank has not reached max rotation
        while (objectAngle < maxRotation)
        {
            // Increase targetRotation by rotationSpeed
            // Round to integer to prevent non-integer angles from deltaTime 
            targetRotation = Mathf.RoundToInt(rotationSpeed * Time.deltaTime);

            // Increase objectAngle by targetRotation
            objectAngle += targetRotation;

            // Rotate plank around given pivot in given direction
            transform.RotateAround(pivot.position, transform.right * direction, targetRotation);

            //int offsetDirection = 1;

            // If Plank is rotating from left pivot
            //if (this.activePivot.name.Equals(plankCollisionDetection.leftPivotName))

            // Inverse camera offset direction
            //offsetDirection = -1;

            // Adjust camera offset
            //var offset = mainCamera.GetComponent<DCameraSmoothFollow>().offset += -.027f * direction * offsetDirection;

            // Returns to top of while loop
            yield return null;
        }

        // Starts a coroutine to disconnect all connected planks
        plankConnection.DisconnectPlanks(this.transform);

        // Sets isRotating to false after Plank has reached max rotation
        this.isRotating = false;
    }
}
