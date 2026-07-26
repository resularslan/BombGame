using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public enum State
    {
        MainMenu,
        StartGame,
        ExitGame
    }
    public State currentState;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentState)
            {
                case State.MainMenu:
                    GameManager.Instance.ReturnToMainMenu();
                    break;
                case State.StartGame:
                    GameManager.Instance.StartGame();
                    break;
                case State.ExitGame:
                    GameManager.Instance.ExitGame();
                    break;
            }
        }
    }
}
