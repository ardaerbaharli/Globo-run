using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class LivesManagerUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> lives;
        
        private List<GameObject> _activeLives;
        private List<GameObject> _lostLives;

        private void Awake()
        {
            _activeLives = new List<GameObject>();
            _lostLives = new List<GameObject>();
            _activeLives = lives.ToList();
        }

        public void RemoveLife()
        {
            if (lives.Count <= 0) return;
            var live = _activeLives.First();
            _lostLives.Add(live);
            _activeLives.Remove(live);
            live.SetActive(false);
        }

        public void AddLife()
        {
            if (_lostLives.Count <= 0) return;
            var live = _lostLives.First();
            _activeLives.Add(live);
            _lostLives.Remove(live);
            live.SetActive(true);
        }
    }
}