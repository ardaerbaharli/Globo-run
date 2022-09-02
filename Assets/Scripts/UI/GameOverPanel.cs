using System.Collections;
using Managers;
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
            // StartCoroutine(ScoreAnimationCoroutine());
            highScoreText.text = $"top score: {ScoreManager.instance.HighScore}";

            soundToggle.Toggle(Config.IsSoundOn);
            vibrationToggle.Toggle(Config.IsVibrationOn);

            soundToggle.OnValueChanged += OnSoundToggleValueChanged;
            vibrationToggle.OnValueChanged += OnVibrationToggleValueChanged;
        }

        public void ScoreAnimation()
        {
            StartCoroutine(ScoreAnimationCoroutine());
        }

        private IEnumerator ScoreAnimationCoroutine()
        {
            var s = ScoreManager.instance.GameOverScore;
            _scoreRevealTime += s / 1000f;
            float score;
            var delta = score = 0f;

            while (delta < _scoreRevealTime)
            {
                delta += Time.deltaTime;
                scoreText.text = $"score: {score:0.}";
                score = Mathf.Lerp(score, s, delta / _scoreRevealTime);
                yield return null;
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