using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class LivesManagerUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> lives;
        
        private List<GameObject> activeLives;
        private List<GameObject> lostLives;

        private void Awake()
        {
            activeLives = new List<GameObject>();
            lostLives = new List<GameObject>();
            activeLives = lives.ToList();
        }

        public void RemoveLife()
        {
            if (lives.Count <= 0) return;
            var live = activeLives.First();
            lostLives.Add(live);
            activeLives.Remove(live);
            live.SetActive(false);
        }

        public void AddLife()
        {
            if (lostLives.Count <= 0) return;
            var live = lostLives.First();
            activeLives.Add(live);
            lostLives.Remove(live);
            live.SetActive(true);
        }
    }
}