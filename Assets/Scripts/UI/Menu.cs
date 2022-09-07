using Enums;
using Managers;
using UnityEngine;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public void StartGame()
        {
            SoundManager.Instance.Play(SoundType.UI);
            GameManager.Instance.StartGame();
        }
    }
}