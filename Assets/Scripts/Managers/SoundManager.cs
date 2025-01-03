using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Audio;
using Utilities;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private DictionaryUnity<SoundType, AudioSource> sounds;

        [SerializeField] private AudioSource uiClick;
        [SerializeField] private AudioSource coinCollect;
        [SerializeField] private AudioSource powerUpCollect;
        [SerializeField] private AudioSource jump;
        [SerializeField] private AudioSource step;
        [SerializeField] private AudioSource brickDrop;
        [SerializeField] private AudioSource hitWall;

        public static SoundManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            SetSound(PlayerPrefsX.GetBool("Sound", true));
        }


        public void SetSound(bool value)
        {
            PlayerPrefsX.SetBool("Sound", value);
            mixer.SetFloat("Master", value ? 0 : -80);
        }


        public void Play(SoundType type)
        {
            sounds[type].Play();
        }
    }
}