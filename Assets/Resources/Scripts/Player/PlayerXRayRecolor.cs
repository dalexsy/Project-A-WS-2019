using UnityEngine;

[ExecuteInEditMode]
public class PlayerXRayRecolor : MonoBehaviour
{
    [SerializeField] Material xRay;
    [SerializeField] Color color = Color.white;

    private void Start()
    {
        // Set x-ray color as color set in editor for this scene
        xRay.SetColor("_BaseColor", color);
    }
}
