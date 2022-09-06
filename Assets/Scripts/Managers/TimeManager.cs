using System;
using Enums;
using NaughtyAttributes;
using UnityEngine;

namespace Managers
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private bool timeLimit;
        [ShowIf("timeLimit")] [SerializeField] private int totalGameTime;

        public int remainingTime { get; private set; }

        public static TimeManager Instance;
        public Action OnTimeUp;

        public delegate void OnTimeChange(int time);

        public OnTimeChange OnTimeChangeCallback;

        public void Awake()
        {
            Instance = this;
            remainingTime = totalGameTime;
        }

        private void Start()
        {
            GameManager.Instance.OnGameStarted += StartTimer;
            GameManager.Instance.OnPaused += PauseTimer;
            GameManager.Instance.OnResumed += StartTimer;
        }

        private void PauseTimer()
        {
            CancelInvoke(nameof(CountDown));
        }

        private void StartTimer()
        {
            if (timeLimit)
                InvokeRepeating(nameof(CountDown), 0, 1); }

        private void CountDown()
        {
            remainingTime--;
            OnTimeChangeCallback?.Invoke(remainingTime);
            if (remainingTime <= 0)
            {
                OnTimeUp?.Invoke();
                CancelInvoke(nameof(CountDown));
            }
        }
    }
}