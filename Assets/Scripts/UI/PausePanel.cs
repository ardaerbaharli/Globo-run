using System;
using System.Collections;
using Dev.ComradeVanti.WaitForAnim;
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
            SoundManager.instance.PlayUiClick();
            GameManager.instance.ResumeGame();
        }


        private void OnSoundToggleValueChanged(bool value)
        {
            SoundManager.instance.PlayUiClick();
            Config.IsSoundOn = value;
        }

        private void OnVibrationToggleValueChanged(bool value)
        {
            SoundManager.instance.PlayUiClick();
            Config.IsVibrationOn = value;
        }
    }
}