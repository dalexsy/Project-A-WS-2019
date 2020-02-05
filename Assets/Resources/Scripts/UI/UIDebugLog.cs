using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Displays debug log on screen.
/// </summary>
public class UIDebugLog : MonoBehaviour
{
    /// <summary>
    /// Message to display on screen.
    /// </summary>
    public string debugMessage;

    public static UIDebugLog instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    private void Update()
    {
        var text = GetComponent<Text>();
        text.text = debugMessage;
    }
}

