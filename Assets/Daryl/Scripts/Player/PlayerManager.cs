using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public bool isMoving = false;
    public bool isRotating = false;

    public float moveSpeed;
    [HideInInspector] public float gravity = 10f;
    [HideInInspector] public int gravityDirection = 1;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float horizontalInput;

    [SerializeField] private bool isUsingGravity = true;
    [SerializeField] private bool isUsingInvertedGravity = false;


    private void Update()
    {
        if (!isUsingGravity) gravity = 0f;
        if (isUsingInvertedGravity) gravityDirection = -1;

        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

}
