using Enums;
using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timeText;

        private LivesManagerUI _livesManagerUI;


        private void Awake()
        {
            _livesManagerUI = GetComponentInChildren<LivesManagerUI>();
        }

        private void Start()
        {
            GameManager.Instance.OnLifeGained += _livesManagerUI.AddLife;
            GameManager.Instance.OnLifeLost += _livesManagerUI.RemoveLife;
            ScoreManager.instance.OnScored += OnScored;
            TimeManager.Instance.OnTimeChangeCallback += OnTimeChange;
        }

        private void OnTimeChange(int time)
        {
            var seconds = Mathf.FloorToInt(time % 60);
            var minutes = Mathf.FloorToInt(time / 60f);
            timeText.text = $"{minutes:00}:{seconds:00}";
        }


        private void OnScored(int score)
        {
            scoreText.text = score.ToString();
        }

        public void Pause()
        {
            SoundManager.Instance.Play(SoundType.UI);
            GameManager.Instance.PauseGame();
        }
    }
}