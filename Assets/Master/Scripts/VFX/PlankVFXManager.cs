using UnityEngine;

public class PlankVFXManager : MonoBehaviour
{
    public GameObject activePivotPrefab;
    public GameObject rotationActivationPrefab;
    [HideInInspector] public Transform playerPivot;
    public static PlankVFXManager instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void ActivePivotVFX(Transform activePivot, bool shouldPlay)
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 offset = new Vector3(0, 0, Offset(activePivot));

        // Flip VFX if using left pivot
        if (activePivot.name.Equals(PlankManager.instance.leftPivotName)) rotation = Quaternion.Euler(0, 0, 180);

        if (shouldPlay)
        {
            if (PlankManager.instance.hasReachedGoal) return;
            UniversalVFXManager.instance.PlayRotatedVFX(activePivot, activePivotPrefab, offset, rotation);
        }

        else
            UniversalVFXManager.instance.StopVFX(activePivot, activePivotPrefab, true);
    }

    public void RotationActivationVFX(Transform activePivot)
    {
        Quaternion rotation = Quaternion.identity;
        Vector3 offset = new Vector3(0, 0, Offset(activePivot));

        // Flip VFX if using left pivot
        if (activePivot.name.Equals(PlankManager.instance.leftPivotName)) rotation = Quaternion.Euler(0, 0, 180);

        UniversalVFXManager.instance.PlayRotatedVFX(activePivot, rotationActivationPrefab, offset, rotation);
    }

    private float Offset(Transform activePivot)
    {
        if (IsOffset(activePivot) == true) return .03f;
        else return 0f;
    }

    private bool IsOffset(Transform activePivot)
    {
        GameObject grandparent = activePivot.parent.gameObject;
        PlankRotation plankRotation = grandparent.GetComponent<PlankRotation>();

        // If bottom of Plank cannot rotate, offset VFX
        if (activePivot.name.Equals(PlankManager.instance.rightPivotName)
                    && !plankRotation.canRotateCounterclockwiseR) return true;

        else if (activePivot.name.Equals(PlankManager.instance.leftPivotName)
            && !plankRotation.canRotateClockwiseL) return true;

        // Otherwise, VFX can stay centered
        else return false;
    }
}