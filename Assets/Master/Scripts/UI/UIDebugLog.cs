using UnityEngine.UI;
using UnityEngine;

public class UIDebugLog : MonoBehaviour
{
    public string debugMessage;

    private void Update()
    {
        var text = GetComponent<Text>();
        text.text = debugMessage;
    }
}

