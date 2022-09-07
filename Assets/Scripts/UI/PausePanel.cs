using Enums;
using Managers;
using UnityEngine;
using Utilities;

namespace UI
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private ToggleSwitch soundToggle, vibrationToggle;
        private Animator _animator;
        private static readonly int Activate = Animator.StringToHash("Activate");
        private static readonly int Deactivate = Animator.StringToHash("Deactivate");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            // _animator.SetTrigger(Activate);
            _animator.SetBool(Activate, true);
            soundToggle.Toggle(Config.IsSoundOn);
            vibrationToggle.Toggle(Config.IsVibrationOn);

            soundToggle.OnValueChanged += OnSoundToggleValueChanged;
            vibrationToggle.OnValueChanged += OnVibrationToggleValueChanged;
        }

        public void ResumeButton()
        {
            _animator.SetBool(Deactivate, true);
        }

        public void ResumeGame()
        {
            SoundManager.Instance.Play(SoundType.UI);
            GameManager.Instance.ResumeGame();
        }


        private void OnSoundToggleValueChanged(bool value)
        {
            SoundManager.Instance.Play(SoundType.UI);
            Config.IsSoundOn = value;
        }

        private void OnVibrationToggleValueChanged(bool value)
        {
            SoundManager.Instance.Play(SoundType.UI);
            Config.IsVibrationOn = value;
        }
    }
}