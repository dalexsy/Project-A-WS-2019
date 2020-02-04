using UnityEngine;

public class ActivePivotVFXResposition : MonoBehaviour
{
    private PlankRotation plankRotation;

    private void Start()
    {
        GameObject grandparent = transform.parent.parent.gameObject;
        plankRotation = grandparent.GetComponent<PlankRotation>();
    }

    private void Update()
    {
        // Offset VFX using Z-axis
        transform.localPosition = new Vector3(0, 0, Mathf.Lerp(transform.localPosition.z, Offset(), .2f));
    }

    private float Offset()
    {
        if (IsOffset() == true) return .031f;
        else return 0f;
    }

    private bool IsOffset()
    {
        // If bottom of Plank cannot rotate, offset VFX
        if (transform.parent.name.Equals(PlankManager.instance.rightPivotName)
                    && !plankRotation.canRotateCounterclockwiseR) return true;

        else if (transform.parent.name.Equals(PlankManager.instance.leftPivotName)
            && !plankRotation.canRotateClockwiseL) return true;

        // Otherwise, VFX can stay centered
        else return false;
    }
}
