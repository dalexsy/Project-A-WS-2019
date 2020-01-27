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

    private Vector3 startPosition;
    private Vector3 offset;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
        offset = new Vector3(0, startPosition.y, 0);
        transform.position = CameraRigRotation.instance.averagePlankPosition + offset;
    }

    private void Update()
    {
        // Keep particle system aligned with current average plank position
        transform.position = Vector3.SmoothDamp(transform.position, CameraRigRotation.instance.averagePlankPosition + offset, ref velocity, repositionRate);
    }
}
