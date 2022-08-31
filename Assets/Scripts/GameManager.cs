using System;
using System.Collections;
using Enums;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;

public class GameManager : MonoBehaviour
{
    public Action OnGameStarted, OnGameOver, OnPaused, OnResumed;
    public Action OnLifeLost, OnLifeGained;
    public static GameManager instance;
    public GameState gameState;
    [SerializeField] private int remainingLives;


    private void Awake()
    {
        instance = this;
        remainingLives = Config.TotalLives;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => ObjectPool.instance.isPoolSet);

        LevelManager.instance.Init();
    }



    public void StartGame()
    {
        gameState = GameState.Playing;
        OnGameStarted?.Invoke();
        PageController.Instance.ShowPage(Pages.Game);
    }

    public void GameOver()
    {
        if (gameState == GameState.GameOver) return;
        gameState = GameState.GameOver;
        OnGameOver?.Invoke();
        // Time.timeScale = 0;
        PageController.Instance.ShowPage(Pages.GameOver);
    }

    public void PauseGame()
    {
        gameState = GameState.Paused;
        PageController.Instance.ShowPage(Pages.Pause);
        OnPaused?.Invoke();
        // Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        gameState = GameState.Playing;
        // Time.timeScale = 1;
        PageController.Instance.ShowPage(Pages.Game);
        OnResumed?.Invoke();
    }

    public void LostLife()
    {
        remainingLives--;
        if (remainingLives <= 0)
            GameOver();
        OnLifeLost?.Invoke();
    }

    public void GainedLife()
    {
        remainingLives++;
        OnLifeGained?.Invoke();
    }
}