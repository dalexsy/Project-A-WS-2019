using UnityEngine;

public class GlowReposition : MonoBehaviour
{
    // Disabled unused variable warning
#pragma warning disable 0414

    [Header("Reposition Rate")]
    [TextArea(0, 5)]
    [SerializeField]
    private string usageNote = "Adjusts the rate at which the particle system repositions itself. Particle system moves towards average Plank position with offset on Y axis.";
    [SerializeField] float repositionRate = 20f;

    private CameraRigRotation cameraRigRotation;
    private Vector3 startPosition;
    private Vector3 offset;
    private Vector3 velocity;

    private void Start()
    {
        cameraRigRotation = GameObject.Find("Camera Rig").GetComponent<CameraRigRotation>();
        startPosition = this.transform.position;
        offset = new Vector3(0, startPosition.y, 0);
        this.transform.position = CameraRigRotation.instance.averagePlankPosition + offset;
    }

    private void Update()
    {
        // Keep particle system aligned with current average plank position
        this.transform.position = Vector3.SmoothDamp(this.transform.position, CameraRigRotation.instance.averagePlankPosition + offset, ref velocity, repositionRate);
    }
}
