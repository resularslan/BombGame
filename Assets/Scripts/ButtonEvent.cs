using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    private enum State
    {
        MainMenu,
        StartGame
    }
    [SerializeField] private State currentState;
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
            }
        }
    }
}
