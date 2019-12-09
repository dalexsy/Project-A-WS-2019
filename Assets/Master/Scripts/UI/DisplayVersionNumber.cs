using UnityEngine.UI;
using UnityEngine;

public class DisplayVersionNumber : MonoBehaviour
{
    private void Start()
    {
        var text = GetComponent<Text>();
        text.text = "V " + Application.version;
    }
}
