using TMPro;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI exitText;
    [SerializeField] private TextMeshProUGUI startText;
    [SerializeField] private float distance;
    private ButtonEvent buttonEvent;

    void Awake()
    {
        buttonEvent = GetComponentInParent<ButtonEvent>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetCursorPosition(exitText);
            buttonEvent.currentState = ButtonEvent.State.ExitGame;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetCursorPosition(startText);
            buttonEvent.currentState = ButtonEvent.State.StartGame;
        }
    }
    private void SetCursorPosition(TextMeshProUGUI targetText)
    {
        float localLeftX = targetText.textBounds.min.x;
        Vector3 worldLeftPoint = targetText.transform.TransformPoint(new Vector3(localLeftX, 0f, 0f));
        transform.position = new Vector3(worldLeftPoint.x - distance, transform.position.y, transform.position.z);
    }
}
