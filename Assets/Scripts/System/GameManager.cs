using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Get(bool AllowCreation = true)
    {
        if (_instance == null && AllowCreation)
        {
            new GameManager().Init();
        }

        return _instance;
    }


    public enum GameState
    {
        MainMenu,
        Playing,
        GameOver
    }

    public GameState currentState;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetState(GameState.MainMenu);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        // Implement behavior based on the new state
        switch (newState)
        {
            case GameState.MainMenu:
                // Show main menu
                break;
            case GameState.Playing:
                // Start or resume the game
                break;
            case GameState.GameOver:
                // Show game over screen
                break;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartGame()
    {
        // Assuming the first scene is your main game scene
        LoadScene("MainScene");
    }
}
