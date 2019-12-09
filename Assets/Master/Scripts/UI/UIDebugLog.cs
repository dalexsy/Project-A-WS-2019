using UnityEngine.UI;
using UnityEngine;

public class UIDebugLog : MonoBehaviour
{
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            var text = GetComponent<Text>();
            text.text = touch.phase.ToString();
        }
    }
}

