using UnityEngine;
using Utilities;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int coinValue;

        public delegate void OnScoredDelegate(int score);

        public event OnScoredDelegate OnScored;
        public static ScoreManager instance;
        private float score;

        public float Score
        {
            get => score;
            set
            {
                score = value;
                if (score > HighScore)
                    HighScore = (int) score;
                OnScored?.Invoke((int) score);
            }
        }

        public int GameOverScore => (int) (score + coinValue * Coin);

        private int coin;

        public int Coin
        {
            get => coin;
            set => coin = value;
        }


        public int HighScore
        {
            get => PlayerPrefs.GetInt(Config.HighScorePref, 0);
            set => PlayerPrefs.SetInt(Config.HighScorePref, value);
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}