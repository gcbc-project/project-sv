using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Get(bool allowCreation = true)
    {
        if (_instance == null && allowCreation)
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

    public GameState CurrentState;
    public GameData Data;

    Dictionary<Type, MonoBehaviour> _monoSingletons = new();

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

        Data.LoadData();
    }

    private void Update()
    {
        Data.Update(Time.deltaTime);
    }

    private void OnDestroy()
    {
        Data.SaveData();
    }

    /// <summary> 
    /// returns false if same type of MonoSingle already exists.
    /// GameObject will be destroyed if false returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="inMonoSingleton"></param>
    /// <returns></returns>
    public bool RegisterMonoSingleton<T>(MonoSingleton<T> inMonoSingleton)
    {
        if (inMonoSingleton == null)
        {
            Debug.LogError("do not register null");
        }

        bool alreadyExists = _monoSingletons.TryGetValue(typeof(T), out MonoBehaviour findee);
        bool registingAgain = findee == inMonoSingleton;

        if (alreadyExists && registingAgain == false)
        {
            Destroy(inMonoSingleton, 0.1f);
        }
        else if (registingAgain == false)
        {
            _monoSingletons.Add(typeof(T), inMonoSingleton);
        }

        return !alreadyExists;
    }

    public void UnregisterMonoSingleton<T>(MonoSingleton<T> inMonoSingleton)
    {
        if (inMonoSingleton == null)
        {
            Debug.LogError("do not unregister null");
        }

        _monoSingletons.TryGetValue(typeof(T), out MonoBehaviour findee);
        if (inMonoSingleton == findee)
        {
            _monoSingletons.Remove(typeof(T));
        }
    }

    public T GetMonoSingleton<T>() where T : MonoSingleton<T>
    {
        return (T)_monoSingletons.GetValueOrDefault(typeof(T));
    }

    public void SetState(GameState newState)
    {
        CurrentState = newState;
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
