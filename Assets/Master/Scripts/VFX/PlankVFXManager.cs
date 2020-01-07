using UnityEngine;

public class PlankVFXManager : MonoBehaviour
{
    public GameObject activePivotPrefab;

    private UniversalVFXManager universalVFXManager;

    private void Start()
    {
        universalVFXManager = GameObject.Find("VFX Manager").GetComponent<UniversalVFXManager>();
    }

    public void ActivePivotVFX(Transform activePivot, bool shouldPlay)
    {
        if (shouldPlay)
            universalVFXManager.PlayVFX(activePivot, activePivotPrefab, Vector3.zero);

        else
            universalVFXManager.StopVFX(activePivot, activePivotPrefab);
    }
}
