using Managers;
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
            GameManager.Instance.OnLifeGained += _livesManagerUI.AddLife;
            GameManager.Instance.OnLifeLost += _livesManagerUI.RemoveLife;
            ScoreManager.instance.OnScored += OnScored;
        }

        private void OnScored(int score)
        {
            scoreText.text = score.ToString();
        }

        public void Pause()
        {
            GameManager.Instance.PauseGame();
        }
    }
}