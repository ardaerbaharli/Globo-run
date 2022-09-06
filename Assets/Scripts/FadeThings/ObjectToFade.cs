using Obstacles;
using UnityEngine;

namespace FadeThings
{
    public class ObjectToFade : MonoBehaviour
    {
        public bool faded;
        private CameraViewFader _cameraViewFader;
        private ObstacleSetup setup;

        private void Awake()
        {
            _cameraViewFader = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraViewFader>();
            setup = GetComponent<ObstacleSetup>();
        }

        private void Update()
        {
            if (_cameraViewFader.objectHit == gameObject)
            {
                if (faded) return;
                setup.FadeOut();
                faded = true;
            }
            else
            {
                if (!faded) return;
                setup.FadeIn();
                faded = false;
            }
        }
    }
}