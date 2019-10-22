using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSmoothiing;
    [SerializeField] float orbitSmoothing;
    Vector3 currentMousePosition;
    Vector3 lastMousePosition;
    Vector3 mouseDelta;
    //wie die maus sich vom letzten frame bis zu diesem frame verändert hat
    Vector3 LastMouseDelta;


    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        FollowTarget();
        OrbitTarget();
    }

    void OrbitTarget()
    {
        currentMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            mouseDelta = lastMousePosition - currentMousePosition;

            mouseDelta = Vector3.Lerp(LastMouseDelta, mouseDelta, orbitSmoothing * Time.deltaTime);

            transform.Rotate(0, -mouseDelta.x, 0, Space.World);
            transform.Rotate(mouseDelta.y, 0, 0, Space.Self);
        }

        if (Input.GetMouseButtonUp(1))
        {
            mouseDelta = Vector3.zero;
        }
        LastMouseDelta = mouseDelta;
        lastMousePosition = currentMousePosition;
    }

    void FollowTarget()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, followSmoothiing * Time.deltaTime);
    }
}

