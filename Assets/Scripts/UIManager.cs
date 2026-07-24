using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;

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
        }
    }
    private IEnumerator DeactivateInformationUI()
    {
        yield return new WaitForSecondsRealtime(deactivateTime);
        stageTextObject.SetActive(false);
        bacground.SetActive(false);
        ActivatePlayingUI();
        gameManager.StartPlaying();
    }
    private void ActivatePlayingUI()
    {
        scoreTextObject.SetActive(true);
        lifeTextObject.SetActive(true);
        timeTextObject.SetActive(true);
        timeText = timeTextObject.GetComponent<TextMeshProUGUI>();
        lifeText = lifeTextObject.GetComponent<TextMeshProUGUI>();
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        SetLifeUI();
        SetScoreUI();
        SetTimeUI();
    }

    private void SetTimeUI()
    {
        string value = Mathf.CeilToInt(gameManager.gameTime).ToString();
        timeText.SetText("TIME: " + value);
    }
    public void SetLifeUI()
    {   
        string value = gameManager.life.ToString();
        lifeText.SetText("LEFT: " + value);
    }
    private void SetScoreUI()
    {   
        string value = gameManager.score.ToString();
        scoreText.SetText(value);
    }

    public void CreateScore(int value, Vector3 position)
    {
        gameManager.AddScore(value);
        SetScoreUI();
        GameObject textObject = ObjectSpawner.Instance.InstantiateObject(score, position, Quaternion.identity);
        TextMeshPro text = textObject.GetComponent<TextMeshPro>();
        string _value = gameManager.topScore.ToString();
        text.SetText(_value);

    }
}
