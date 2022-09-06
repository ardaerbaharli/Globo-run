using Enums;
using Managers;
using UnityEngine;

namespace FadeThings
{
    public class CameraViewFader : MonoBehaviour
    {
        [HideInInspector] public GameObject objectHit;

        private GameObject player;

        private void Awake()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            if (GameManager.Instance.gameState != GameState.Playing) return;

            // Every 10 frame check if the player is looking at an object
            if (Time.frameCount % 10 == 0)
            {
                var direction = (-transform.position + player.transform.position).normalized;
                var ray = new Ray(transform.position, direction);
                if (Physics.Raycast(ray, out var hit, 50f))
                {
                    objectHit = hit.transform.gameObject;

                    var findParent = hit.transform.GetComponentInParent<ObjectToFade>();
                    if (findParent == null) return;
                    objectHit = findParent.gameObject;

                    Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                }

                Debug.DrawRay(transform.position, direction * 50, Color.red);
            }
        }
    }
}