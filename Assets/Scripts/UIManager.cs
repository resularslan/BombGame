using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; } 
    [SerializeField] private float deactivateTime = 2f;
    [SerializeField] private GameObject stageTextObject;
    [SerializeField] private GameObject bacground;
    [SerializeField] private GameObject timeTextObject;
    [SerializeField] private GameObject lifeTextObject;
    [SerializeField] private GameObject scoreTextObject;
    [SerializeField] private GameObject score;
    private TextMeshProUGUI stageText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI lifeText;
    private TextMeshProUGUI scoreText;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        stageText = stageTextObject.GetComponent<TextMeshProUGUI>();
        stageText.SetText(SceneManager.GetActiveScene().name);
        StartCoroutine(DeactivateInformationUI());
    }
    void Update()
    {
        if (GameManager.Instance.isPlaying)
        {
            SetTimeUI();
            SetLifeUI();
            SetScoreUI();
        }
    }
    private IEnumerator DeactivateInformationUI()
    {
        yield return new WaitForSecondsRealtime(deactivateTime);
        stageTextObject.SetActive(false);
        bacground.SetActive(false);
        GameManager.Instance.StartPlaying();
    }

    private void SetTimeUI()
    {
        timeTextObject.SetActive(true);
        timeText = timeTextObject.GetComponent<TextMeshProUGUI>();
        string value = Mathf.CeilToInt(GameManager.Instance.gameTime).ToString();
        timeText.SetText("TIME: " + value);
    }
    private void SetLifeUI()
    {
        lifeTextObject.SetActive(true);
        lifeText = lifeTextObject.GetComponent<TextMeshProUGUI>();
        string value = GameManager.Instance.life.ToString();
        lifeText.SetText("LEFT: " + value);
    }
    private void SetScoreUI()
    {
        scoreTextObject.SetActive(true);
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        string value = GameManager.Instance.score.ToString();
        scoreText.SetText(value);
    }

    public void CreateScore(int value, Vector3 position)
    {
        GameManager.Instance.AddScore(value);
        TextMeshProUGUI _text = score.GetComponentInChildren<TextMeshProUGUI>();
        _text.SetText(value.ToString());
        ObjectSpawner.Instance.InstantiateObject(score, position, Quaternion.identity);
    }
}
