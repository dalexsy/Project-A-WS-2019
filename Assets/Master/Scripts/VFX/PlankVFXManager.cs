using UnityEngine;

public class PlankVFXManager : MonoBehaviour
{
    public GameObject activePivotPrefab;
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
}