using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    private enum State
    {
        MainMenu,
        GameOver
    }
    [SerializeField] private State currentState;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            switch (currentState)
            {
                case State.GameOver:
                    GameManager.Instance.ReturnToMainMenu();
                    break;
                case State.MainMenu:
                    GameManager.Instance.StartGame();
                    break;
            }
        }
    }
}
