using UnityEngine;

[ExecuteInEditMode]
public class PlayerXRayRecolor : MonoBehaviour
{
    [SerializeField] Material xRay;
    [SerializeField] Color color;

    private void Start()
    {
        xRay.SetColor("_BaseColor", color);
    }
}
