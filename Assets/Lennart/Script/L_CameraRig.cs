using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L_CameraRig : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float followSmoothing;
    [SerializeField] float orbitSmoothing;


    Vector3 currentMousePosition;
    Vector3 lastMousePosition;
    Vector3 mouseDelta;
    Vector3 lastMouseDelta;


    void Start()
    {
        
    }

 
    void Update()
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

            mouseDelta = Vector3.Lerp(lastMouseDelta, mouseDelta, orbitSmoothing * Time.deltaTime);

            transform.Rotate(0, -mouseDelta.x, 0, Space.World);
            transform.Rotate(mouseDelta.y, 0, 0, Space.Self);
        }

        if (Input.GetMouseButtonUp(1))
        {
            mouseDelta = Vector3.zero;
        }

        lastMouseDelta = mouseDelta;
        lastMousePosition = currentMousePosition;
    }

    void FollowTarget()
    {                                   //Lerp der wert wo ich bin / der wert wo ich hinwill /
        this.transform.position = Vector3.Lerp(transform.position, target.position, followSmoothing * Time.deltaTime);
    }
}
