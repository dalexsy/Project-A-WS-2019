using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPlankRotation : MonoBehaviour
{
    [SerializeField] float maxRotation = 90f;
    [SerializeField] float rotationSpeed = 30f;

    [SerializeField] GameObject pulseParticlePrefab = null;

    public Transform activePivot = null;
    public Transform surrogatePivot = null;

    public bool canRotateClockwise = true;
    public bool canRotateCounterclockwise = true;

    private bool isRotating = false;

    private CollisionDetection collisionDetection;
    private PlankCollisionDetection plankCollisionDetection;
    private PlankConnection plankConnection;

    private float objectAngle = 0f;
    private float targetRotation = 0f;

    private GameObject mainCamera;

    private IEnumerator connectionCoroutine;

    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        collisionDetection = GetComponent<CollisionDetection>();
        plankCollisionDetection = GetComponentInChildren<PlankCollisionDetection>();
        plankConnection = GetComponent<PlankConnection>();
    }

    private void Update()
    {
        // If Plank is not colliding with Player
        if (collisionDetection.isCollidingWithTarget == false)

            // Accept rotation input
            RotationInput();
    }

    private void RotationInput()
    {
        // If no pivots are given, accept no input
        if (!activePivot)
            return;

        // If Plank is not rotating
        if (!isRotating)
        {
            // If Plank can rotate clockwise
            if (canRotateClockwise)
            {
                // Rotate plank clockwise from active pivot
                if (Input.GetKeyDown("e"))
                {
                    StartCoroutine(RotatePlank(1, activePivot));
                }
            }

            //  If Plank can rotate counterclockwise
            if (canRotateCounterclockwise)
            {
                // Rotate plank counterclockwise from active pivot
                if (Input.GetKeyDown("q"))
                {
                    StartCoroutine(RotatePlank(-1, activePivot));
                }
            }
        }
    }

    // Rotates Plank
    // Requires direction (1 for down, -1 for up) and pivot (lPivot, rPivot)
    IEnumerator RotatePlank(int direction, Transform pivot)
    {
        Debug.Log(transform.name + " is rotating");
        // Save local variable rotationPivot from active pivot
        // Needed in case Player leaves range of pivot during coroutine and pivot is unassigned 
        Transform rotationPivot = pivot;
        Transform surrogateRotationPivot = null;
        Vector3 rotationAxis = transform.TransformDirection(Vector3.left);

        // If a surrogate pivot has been assigned
        if (surrogatePivot)
        {
            Debug.Log("surrogate1");

            // Save local variable surrogateRotationPivot as surrogatePivot
            surrogateRotationPivot = surrogatePivot;
        }

        // Start coroutine to connect planks using active pivot
        StartCoroutine(plankConnection.ConnectPlanks(rotationPivot));

        // Reset object angle
        objectAngle = 0f;

        // Set isRotating to true to prevent multiple rotations
        this.isRotating = true;

        if (surrogateRotationPivot)
        {
            rotationPivot = surrogateRotationPivot;
            rotationAxis = transform.TransformDirection(Vector3.forward);
            direction = direction * -1;
            Debug.Log("surrogate2");
        }

        // Create visual feedback on pivot to be rotated from
        GameObject pulse = Instantiate(pulseParticlePrefab, rotationPivot.transform.position, pulseParticlePrefab.transform.rotation);

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
            transform.RotateAround(rotationPivot.position, rotationAxis * direction, targetRotation);

            int offsetDirection = -1;

            // If Plank is rotating from left pivot
            if (rotationPivot.name.Equals(plankCollisionDetection.leftPivotName))

                // Inverse camera offset direction
                offsetDirection = 1;

            // Adjust camera offset
            var offset = mainCamera.GetComponent<DCameraSmoothFollow>().offset += -.027f * direction * offsetDirection;

            // Returns to top of while loop
            yield return null;
        }

        // Starts a coroutine to disconnect all connected planks
        StartCoroutine(plankConnection.DisconnectPlanks(this.transform));

        // Sets isRotating to false after Plank has reached max rotation
        this.isRotating = false;
    }
}
