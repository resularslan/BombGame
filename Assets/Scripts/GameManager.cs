using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    float gameTime = 200;
    public static event Action OnTimeZero;
    bool isTimeUp = false;
    void OnEnable()
    {
        PlayerController.OnPlayerDied += RestartGame;
    }
    void OnDisable()
    {
        PlayerController.OnPlayerDied -= RestartGame;
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
            OnTimeZero?.Invoke();
        }
    }
    private void RestartGame()
    {
        StartCoroutine(LoadScene());
    }
    private IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Stage-1");
    }
}
