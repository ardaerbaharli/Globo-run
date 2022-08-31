using UnityEngine;
using UnityEngine.Audio;
using Utilities;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private AudioSource uiClick;


    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        return;
        SetSound(PlayerPrefsX.GetBool("Sound", true));
    }


    public void SetSound(bool value)
    {
        PlayerPrefsX.SetBool("Sound", value);
        mixer.SetFloat("Master", value ? 0 : -80);
    }


    public void PlayUiClick()
    {
        return;
        uiClick.Play();
    }
}