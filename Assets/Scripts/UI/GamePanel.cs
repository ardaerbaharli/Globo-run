using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;

        private LivesManagerUI _livesManagerUI;
        

        private void Awake()
        {
            _livesManagerUI = GetComponentInChildren<LivesManagerUI>();
        }

        private void Start()
        {
            GameManager.instance.OnLifeGained += _livesManagerUI.AddLife;
            GameManager.instance.OnLifeLost += _livesManagerUI.RemoveLife;
            ScoreManager.instance.OnScored += OnScored;
        }

        private void OnScored(int score)
        {
            scoreText.text = score.ToString();
        }

        public void Pause()
        {
            GameManager.instance.PauseGame();
        }
    }
}