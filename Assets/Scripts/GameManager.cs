using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private float gameTime = 200f;
    public static event Action OnTimeUp;
    private bool isTimeUp = false;
    private int life = 2;

    void Awake()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("GameController");
        if (objects.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void OnEnable()
    {
        PlayerController.OnPlayerDied += HandleScene;
    }
    void OnDisable()
    {
        PlayerController.OnPlayerDied -= HandleScene;
    }

    private void Update()
    {
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
        if (life >= 0)
        {
            life--;
            StartCoroutine(LoadScene(SceneManager.GetActiveScene().name, 2f));
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
    }
    private IEnumerator LoadScene(string scene, float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        SceneManager.LoadScene(scene);
    }

    public void StartGame()
    {
        StartCoroutine(LoadScene("Stage-1", 0f));
    }

    public void ReturnToMainMenu()
    {
        StartCoroutine(LoadScene("MainMenu", 0f));
    }
}
