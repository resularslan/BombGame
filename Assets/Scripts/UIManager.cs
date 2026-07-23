using System;
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
    private GameManager gameManager;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gameManager = GameManager.Instance;
        stageText = stageTextObject.GetComponent<TextMeshProUGUI>();
        stageText.SetText(SceneManager.GetActiveScene().name);
        StartCoroutine(DeactivateInformationUI());
    }
    void Update()
    {
        if (gameManager.isPlaying)
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
        gameManager.StartPlaying();
    }

    private void SetTimeUI()
    {
        timeTextObject.SetActive(true);
        timeText = timeTextObject.GetComponent<TextMeshProUGUI>();
        string value = Mathf.CeilToInt(gameManager.gameTime).ToString();
        timeText.SetText("TIME: " + value);
    }
    private void SetLifeUI()
    {
        lifeTextObject.SetActive(true);
        lifeText = lifeTextObject.GetComponent<TextMeshProUGUI>();
        string value = gameManager.life.ToString();
        lifeText.SetText("LEFT: " + value);
    }
    private void SetScoreUI()
    {
        scoreTextObject.SetActive(true);
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        string value = gameManager.score.ToString();
        scoreText.SetText(value);
    }

    public void CreateScore(int value, Vector3 position)
    {
        gameManager.AddScore(value);
        GameObject textObject = ObjectSpawner.Instance.InstantiateObject(score, position, Quaternion.identity);
        TextMeshPro text = textObject.GetComponent<TextMeshPro>();
        string _value = gameManager.topScore.ToString();
        text.SetText(_value);

    }
}
