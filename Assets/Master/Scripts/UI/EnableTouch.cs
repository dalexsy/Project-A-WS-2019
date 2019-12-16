using UnityEngine.UI;
using UnityEngine;

public class EnableTouch : MonoBehaviour
{
    private Button button;
    private InputManager inputManager;
    [SerializeField] private Sprite touchIcon = null;
    [SerializeField] private Sprite mouseIcon = null;

    private void Start()
    {
        button = GetComponent<Button>();
        inputManager = GameObject.Find("Input Manager").GetComponent<InputManager>();
    }

    private void Update()
    {
        if (touchIcon && inputManager.isUsingTouch) button.GetComponent<Image>().sprite = touchIcon;
        if (mouseIcon && !inputManager.isUsingTouch) button.GetComponent<Image>().sprite = mouseIcon;
    }

    public void TouchToggle()
    {
        inputManager.isUsingTouch = !inputManager.isUsingTouch;
    }
}
