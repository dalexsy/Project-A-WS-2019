using UnityEngine;

public class GlowReposition : MonoBehaviour
{
    private CameraRigRotation cameraRigRotation;
    private Vector3 startPosition;

    private void Start()
    {
        cameraRigRotation = GameObject.Find("Camera Rig").GetComponent<CameraRigRotation>();
        startPosition = this.transform.position;
    }

    private void Update()
    {
        // Keep particle system aligned with current average plank position
        this.transform.position = cameraRigRotation.averagePlankPosition + startPosition;
    }
}
