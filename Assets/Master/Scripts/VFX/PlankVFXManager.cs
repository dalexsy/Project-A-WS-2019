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
        if (shouldPlay)
            UniversalVFXManager.instance.PlayVFX(activePivot, activePivotPrefab, Vector3.zero);

        else
            UniversalVFXManager.instance.StopVFX(activePivot, activePivotPrefab);
    }
}
