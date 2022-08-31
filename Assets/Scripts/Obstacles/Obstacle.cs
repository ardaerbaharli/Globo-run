using System;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Obstacles
{
    public class Obstacle : MonoBehaviour
    {
        private void Awake()
        {
            var renderer = GetComponent<Renderer>();
            var num = Random.Range(0, 5);
            renderer.material = Resources.Load<Material>("Materials/" + num);
        }


        private void Start()
        {
            GameManager.instance.OnPaused += OnPaused;
            GameManager.instance.OnResumed += OnResumed;
        }

        private void OnResumed()
        {
            iTween.Resume(gameObject);
        }

        private void OnPaused()
        {
            iTween.Pause(gameObject);
        }

        private Vector3 startPosition;

        public virtual void SetStartPosition()
        {
            startPosition = transform.localPosition;
        }
        
        public virtual void ResetPosition()
        {
            transform.localPosition = startPosition;
        }
    }
}