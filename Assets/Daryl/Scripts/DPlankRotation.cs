using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DPlankRotation : MonoBehaviour
{
    [SerializeField] float maxRotation = 90f;
    [SerializeField] float rotationSpeed = 30f;

    [SerializeField] GameObject pulseParticlePrefab = null;

    [SerializeField] Transform lPivot = null;
    [SerializeField] Transform rPivot = null;

    private GameObject mainCamera;

    private bool isRotating = false;

    private float objectAngle = 0f;
    private float targetRotation = 0f;

    private void Start()
    {
        mainCamera = GameObject.Find("Main Camera");
    }

    private void Update()
    {
        RotationInput();
    }

    private void RotationInput()
    {
        // If no pivots are given, accept no input
        if (!lPivot || !rPivot)
            return;

        // If Plank is not rotating
        if (!isRotating)
        {
            // Rotate plank up from left pivot
            if (Input.GetKeyDown("q"))
            {
                StartCoroutine(RotatePlank(-1, lPivot));
            }

            // Rotate plank down from left pivot
            if (Input.GetKeyDown("e"))
            {
                StartCoroutine(RotatePlank(1, lPivot));
            }

            // Rotate plank up from right pivot
            if (Input.GetKeyDown("i"))
            {
                StartCoroutine(RotatePlank(-1, rPivot));
            }

            // Rotate plank down from right pivot
            if (Input.GetKeyDown("p"))
            {
                StartCoroutine(RotatePlank(1, rPivot));
            }
        }
    }

    // Rotates Plank
    // Requires direction (1 for down, -1 for up) and pivot (lPivot, rPivot)
    IEnumerator RotatePlank(int direction, Transform pivot)
    {
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

            // Adjust camera offset
            var offset = mainCamera.GetComponent<DCameraSmoothFollow>().offset += -.027f * direction;

            // Returns to top of while loop
            yield return null;
        }

        // Sets isRotating to false after Plank has reached max rotation
        this.isRotating = false;
    }

}
