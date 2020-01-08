using UnityEngine.UI;
using UnityEngine;

public class DisplayMoveCounter : MonoBehaviour
{
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = MoveCounter.instance.moveCount.ToString();
    }
}

