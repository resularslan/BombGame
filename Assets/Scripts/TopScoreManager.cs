using TMPro;
using UnityEngine;

public class TopScoreManager : MonoBehaviour
{
    TextMeshProUGUI topText;
    void Awake()
    {
        topText = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        SetMaxScoreUI();
    }
    private void SetMaxScoreUI()
    {
        int score = GameManager.Instance.topScore;
        string value = score.ToString();
        topText.SetText("TOP: " + value);
    }
}
