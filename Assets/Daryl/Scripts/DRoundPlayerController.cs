using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DRoundPlayerController : MonoBehaviour
{
    private float moveX;
    private float moveY;

    private Rigidbody rigid;
    private Vector3 moveDirection;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Gets horizontal input
        moveX = Input.GetAxis("kHorizontal");

        // Gets vertical input
        moveY = Input.GetAxis("kVertical");
    }
}
