using UnityEngine.UI;
using UnityEngine;

public class EnableTouch : MonoBehaviour
{
    private Button button;
    [SerializeField] private Sprite touchIcon = null;
    [SerializeField] private Sprite mouseIcon = null;

    private void Start()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (touchIcon && InputManager.instance.isUsingTouch) button.GetComponent<Image>().sprite = touchIcon;
        if (mouseIcon && !InputManager.instance.isUsingTouch) button.GetComponent<Image>().sprite = mouseIcon;
    }

    public void TouchToggle()
    {
        InputManager.instance.isUsingTouch = !InputManager.instance.isUsingTouch;
    }
}
