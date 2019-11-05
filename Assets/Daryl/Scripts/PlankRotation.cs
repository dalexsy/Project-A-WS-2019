using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankRotation : MonoBehaviour
{
    [SerializeField] float maxRotation = 90f;
    [SerializeField] float rotationSpeed = 30f;

    [SerializeField] GameObject pulseParticlePrefab = null;

    public Transform activePivot = null;
    public Transform surrogatePivot = null;

    public bool canRotateClockwiseR = true;
    public bool canRotateCounterclockwiseR = true;
    public bool canRotateClockwiseL = true;
    public bool canRotateCounterclockwiseL = true;
    public bool isConnectedFront = false;
    public bool isConnectedBack = false;

    private bool isRotating = false;

    private CollisionDetection collisionDetection;
    private PivotAssignment pivotAssignment;
    private PlankConnection plankConnection;

    private float objectAngle = 0f;
    private float targetRotation = 0f;

    private GameObject mainCamera;

    private IEnumerator connectionCoroutine;

    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
        collisionDetection = GetComponent<CollisionDetection>();
        pivotAssignment = GetComponentInChildren<PivotAssignment>();
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
            if (canRotateClockwiseR && activePivot.name.Equals("Pivot R") ||
                canRotateClockwiseL && activePivot.name.Equals("Pivot L"))
            {
                if (Input.GetKeyDown("e") ||
                    Input.GetKey(KeyCode.Joystick1Button6))
                {
                    // If Plank is connected front (requires surrogate pivot)
                    if (isConnectedFront)
                    {
                        // Rotate Plank clockwise
                        // Will rotate from surrogate pivot's position
                        StartCoroutine(RotatePlank(1, activePivot));
                    }

                    else
                    {
                        // Rotate Plank clockwise 
                        // Will rotate from active pivot's position
                        StartCoroutine(RotatePlank(-1, activePivot));
                    }
                }
            }

            //  If Plank can rotate counterclockwise
            if (canRotateCounterclockwiseR && activePivot.name.Equals("Pivot R") ||
                canRotateCounterclockwiseL && activePivot.name.Equals("Pivot L"))
            {
                if (Input.GetKeyDown("q") ||
                    Input.GetKey(KeyCode.Joystick1Button7))
                {
                    // Rotate Plank counterclockwise
                    // Will rotate from surrogate pivot's position
                    if (isConnectedFront)
                    {
                        StartCoroutine(RotatePlank(-1, activePivot));
                    }

                    // Rotate Plank clockwise
                    // Will rotate from active pivot's position
                    else
                    {
                        StartCoroutine(RotatePlank(1, activePivot));
                    }
                }
            }
        }
    }

    // Rotates Plank
    // Requires direction and pivot (lPivot, rPivot)
    IEnumerator RotatePlank(int direction, Transform pivot)
    {
        // Save local variable rotationPivot from active pivot
        // Needed in case Player leaves range of pivot during coroutine and pivot is unassigned 
        Transform rotationPivot = pivot;
        Transform surrogateRotationPivot = null;
        Vector3 rotationAxis = rotationPivot.transform.right;

        // If a surrogate pivot has been assigned
        if (surrogatePivot)
        {
            // Save local variable surrogateRotationPivot as surrogatePivot
            surrogateRotationPivot = surrogatePivot;
        }

        // Reset object angle
        objectAngle = 0f;

        // Set isRotating to true to prevent multiple rotations
        this.isRotating = true;

        // Start coroutine to connect planks using rotation pivot
        plankConnection.ConnectPlanks(rotationPivot);

        // If using a surrogate pivot
        if (surrogatePivot)
        {
            // Assign surrogate pivot as rotationpivot
            rotationPivot = surrogateRotationPivot;

            // Redefines rotationAxis using surrogate pivot
            rotationAxis = rotationPivot.transform.right;
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

            // Returns to top of while loop
            yield return null;
        }

        // Disconnect all connected planks
        plankConnection.DisconnectPlanks(this.transform);

        // Sets isRotating to false after Plank has reached max rotation
        this.isRotating = false;
    }
}
