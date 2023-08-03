using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [SerializeField] private State state_of_game;
    [SerializeField] private GameObject pause_menu;
    private static GameState instance;
    [SerializeField] private bool is_playing = true;

    public static GameState Instance
    {
        get
        {
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        is_playing = true;
        pause_menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TurnAction();
    }

    private void TurnAction()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (state_of_game.GetState())
            {
                pause_menu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                pause_menu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            state_of_game.ToogleState();
            is_playing = state_of_game.GetState();
        }
    }

    public bool GetGameState()
    {
        return is_playing;
    }

    public void ResumeButtonPress()
    {
        pause_menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        is_playing = true;
        state_of_game.SetState(true);
    }

    public void MainMenuButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void StartButtonPressed()
    {
        SceneManager.LoadScene(1);
    }

    

}
