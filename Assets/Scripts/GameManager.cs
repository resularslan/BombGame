using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float gameTime { get; private set; } = 200f;
    public static event Action OnTimeUp;
    private bool isTimeUp = false;
    public int life { get; private set; } = 2;
    public int score { get; private set; } = 0;
    public bool isPlaying { get; private set; } = false;
    public int topScore { get; private set; } 

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        topScore = PlayerPrefs.GetInt("TopScore");
    }
    void OnEnable()
    {
        PlayerController.OnPlayerDied += HandleScene;
    }
    void OnDisable()
    {
        PlayerController.OnPlayerDied -= HandleScene;
        if (Instance == this) 
        {
            Instance = null;
        }
    }

    private void Update()
    {
        if (!isPlaying) return;
        if (gameTime > 0)
        {
            gameTime -= Time.deltaTime;
        }
        else if (!isTimeUp)
        {
            gameTime = 0f;
            isTimeUp = true;
            OnTimeUp?.Invoke();
        }
    }
    private void HandleScene()
    {
        if (life > 0)
        {
            Time.timeScale = 0;
            life--;
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().name, 2f));
            gameTime = 200f;
            isPlaying = false;
        }
        else
        {
            GameOver();
        }
    }
    private void GameOver()
    {
        Time.timeScale = 0;
        StartCoroutine(LoadScene("GameOver", 2f));
        isPlaying = false;
    }
    private IEnumerator LoadScene(string scene, float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene(scene);
    }
    private void SaveScore()
    {
        PlayerPrefs.SetInt("TopScore", topScore);
        PlayerPrefs.Save();
    }

    public void StartPlaying()
    {
        isPlaying = true;
        Time.timeScale = 1;
        UIManager.Instance.SetLifeUI();
    }
    public void AddScore(int value)
    {
        score += value;
    }

    public void StartGame()
    {
        StartCoroutine(LoadScene("Stage-1", 0f));
    }

    public void ReturnToMainMenu()
    {
        isPlaying = false;
        StartCoroutine(LoadScene("MainMenu", 0f));
    }
    public void CallWin()
    {
        StartCoroutine(Win());
    }
    public IEnumerator Win()
    {
        topScore = Math.Max(score, topScore);
        SaveScore();
        UIManager.Instance.ActivateWinningUI();
        Time.timeScale = 0f;
        yield return new WaitForSeconds(2f);
        ReturnToMainMenu();
        SaveScore();
    }
}
