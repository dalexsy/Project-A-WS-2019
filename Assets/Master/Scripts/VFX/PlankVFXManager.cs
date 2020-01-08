using UnityEngine;

public class PlankVFXManager : MonoBehaviour
{
    public GameObject activePivotPrefab;
    public GameObject rotationActivationPrefab;
    public static PlankVFXManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void ActivePivotVFX(Transform activePivot, bool shouldPlay)
    {
        var rotation = Quaternion.identity;

        // Flip VFX if using left pivot
        if (activePivot.name.Equals(PlankManager.instance.leftPivotName)) rotation = Quaternion.Euler(0, 0, 180);


        if (shouldPlay)
            UniversalVFXManager.instance.PlayVFX(activePivot, activePivotPrefab, Vector3.zero, rotation);

        else
            UniversalVFXManager.instance.StopVFX(activePivot, activePivotPrefab);
    }

    public void RotationActivationVFX(Transform activePivot)
    {
        var rotation = Quaternion.identity;

        // Flip VFX if using left pivot
        if (activePivot.name.Equals(PlankManager.instance.leftPivotName)) rotation = Quaternion.Euler(0, 0, 180);

        UniversalVFXManager.instance.PlayVFX(activePivot, rotationActivationPrefab, Vector3.zero, rotation);
    }
}