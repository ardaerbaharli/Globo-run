using System;
using System.Collections;
using Enums;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public Action OnGameStarted, OnGameOver, OnPaused, OnResumed;
        public Action OnLifeLost, OnLifeGained;
        public static GameManager Instance;
        public GameState gameState;

        public int remainingLives;


        private void Awake()
        {
            Instance = this;
            remainingLives = Config.TotalLives;
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => ObjectPool.Instance.isPoolSet);

            LevelManager.Instance.Init();
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
            Vibration.Heavy();
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
            Vibration.Medium();
            remainingLives--;
            if (remainingLives <= 0)
                GameOver();
            OnLifeLost?.Invoke();
        }

        public void GainedLife()
        {
            if (remainingLives >= 3) return;
            remainingLives++;
            OnLifeGained?.Invoke();
        }
    }
}