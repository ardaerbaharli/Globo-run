using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private ToggleSwitch soundToggle, vibrationToggle;

        private float _scoreRevealTime = 1f;

        private void OnEnable()
        {
            StartCoroutine(ScoreAnimation());
            highScoreText.text = $"top score: {ScoreManager.instance.HighScore}";

            soundToggle.Toggle(Config.IsSoundOn);
            vibrationToggle.Toggle(Config.IsVibrationOn);

            soundToggle.OnValueChanged += OnSoundToggleValueChanged;
            vibrationToggle.OnValueChanged += OnVibrationToggleValueChanged;
        }

        private IEnumerator ScoreAnimation()
        {
            var score = ScoreManager.instance.Score;
            _scoreRevealTime += score % 50;
            
            var s = 0;
            var interval = _scoreRevealTime / score;
            while (score - s != -1)
            {
                scoreText.text = $"score: {s}";
                s++;
                yield return new WaitForSeconds(interval);
            }
        }

        public void RestartButton()
        {
            SoundManager.Instance.PlayUiClick();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnSoundToggleValueChanged(bool value)
        {
            SoundManager.Instance.PlayUiClick();
            Config.IsSoundOn = value;
        }

        private void OnVibrationToggleValueChanged(bool value)
        {
            SoundManager.Instance.PlayUiClick();
            Config.IsVibrationOn = value;
        }
    }
}