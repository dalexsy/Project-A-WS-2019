using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public GameObject target;

    void LateUpdate()
    {
        // Face camera towards target
        transform.LookAt(target.transform);
    }
}

