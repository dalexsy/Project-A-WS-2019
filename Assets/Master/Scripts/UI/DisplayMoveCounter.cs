using UnityEngine.UI;
using UnityEngine;

public class DisplayMoveCounter : MonoBehaviour
{
    private MoveCounter moveCounter;
    private Text text;

    private void Start()
    {
        moveCounter = GameObject.Find("Game Manager").GetComponent<MoveCounter>();
        text = GetComponent<Text>();
    }

    private void Update()
    {
        text.text = moveCounter.moveCount.ToString();
    }
}

