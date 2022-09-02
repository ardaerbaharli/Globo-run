using Managers;
using UnityEngine;

namespace UI
{
    public class Menu : MonoBehaviour
    {
        public void StartGame()
        {
            GameManager.Instance.StartGame();
        }
    }
}