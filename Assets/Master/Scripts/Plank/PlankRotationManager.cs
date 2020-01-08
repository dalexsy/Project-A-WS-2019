using UnityEngine;

public class PlankRotationManager : MonoBehaviour
{
    public static PlankRotationManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public AnimationCurve animationCurve;
    public float maxRotation = 90f;
    [Range(0, 1)] public float rotationSpeed = 1f;
    public bool isRotating = false;
    public GameObject rotateParticlePrefab = null;
    public GameObject activePivotParticlePrefab = null;
}
