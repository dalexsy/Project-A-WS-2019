using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Displays version number on screen.
/// </summary>
[ExecuteInEditMode]
public class DisplayVersionNumber : MonoBehaviour
{
    private void Start()
    {
        var text = GetComponent<Text>();
        text.text = "V " + Application.version;
    }
}
