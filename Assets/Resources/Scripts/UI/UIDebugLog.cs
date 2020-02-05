using UnityEngine.UI;
using UnityEngine;

public class UIDebugLog : MonoBehaviour
{
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

