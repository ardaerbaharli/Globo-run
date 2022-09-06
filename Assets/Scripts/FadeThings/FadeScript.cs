using System.Collections;
using UnityEngine;

namespace FadeThings
{
    [RequireComponent(typeof(MeshRenderer))]
    public class FadeScript : MonoBehaviour
    {
        enum FadeDirection
        {
            In,
            Out
        }

        private FadeDirection theDirectionToFade;

        public Color FadeColor = Color.white;
        public float FadeDuration = 0.25f;
        private Material fadeMaterial;

        void Start()
        {
            fadeMaterial = GetComponent<Renderer>().material;
            var tempColor = fadeMaterial.GetColor("_BaseColor");
            tempColor.a = 0f;
            fadeMaterial.SetColor("_BaseColor", tempColor);
            fadeMaterial.color = tempColor;
        }

        /// <summary>
        /// This method to take the viewer from a fully obscured starting color (i.e. all they see if the color), to
        /// the scene itself
        /// </summary>
        public void FadeIn()
        {
#if UNITY_EDITOR
            fadeMaterial = GetComponent<Renderer>().material;
#endif
            theDirectionToFade = FadeDirection.In;
            StartCoroutine(FadeOverTime(FadeDuration, theDirectionToFade));
        }

        /// <summary>
        /// This method to take the viewer from the scene view to a fully obscured end color color (i.e. all they see if the color), to
        /// the scene itself
        /// </summary>
        public void FadeOut()
        {
#if UNITY_EDITOR
            fadeMaterial = GetComponent<Renderer>().material;
#endif
            theDirectionToFade = FadeDirection.Out;
            StartCoroutine(FadeOverTime(FadeDuration, theDirectionToFade));
        }

        IEnumerator FadeOverTime(float duration, FadeDirection direction)
        {
            var StartColor = FadeColor;
            var EndColor = StartColor;
            StartColor.a = (direction == FadeDirection.In) ? 1 : 0;
            EndColor.a = (direction == FadeDirection.In) ? 0 : 1;

            for (float t = 0f; t < duration; t += Time.deltaTime)
            {
                float normalizedTime = t / duration;
                fadeMaterial.SetColor("_BaseColor", Color.Lerp(StartColor, EndColor, normalizedTime));
                yield return null;
            }

            fadeMaterial.SetColor("_BaseColor", EndColor); //without this, the value will end at something like 0.9992367
        }
    }
}