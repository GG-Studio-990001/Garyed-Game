using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.InGameSystem
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDeltaTime = 0.01f;

        public float FadeDuration { get; set; } = 1f;
        private Coroutine currentFadeCoroutine;

        public void StartFadeIn()
        {
            StopCurrentFadeCoroutine();
            
            currentFadeCoroutine = StartCoroutine(FadeInCoroutine(FadeDuration));
        }

        public void StartFadeOut()
        {
            StopCurrentFadeCoroutine();
            
            currentFadeCoroutine = StartCoroutine(FadeOutCoroutine(FadeDuration));
        }

        public void SetBackground(bool isBlack)
        {
            StopCurrentFadeCoroutine();
            if (isBlack)
                fadeImage.color = Color.black;
            else
                fadeImage.color = Color.clear;
        }

        private void StopCurrentFadeCoroutine()
        {
            if (currentFadeCoroutine != null)
            {
                StopCoroutine(currentFadeCoroutine);
                currentFadeCoroutine = null;
            }
        }

        private IEnumerator FadeInCoroutine(float duration)
        {
            float time = 0;
            while (time < duration)
            {
                time += fadeDeltaTime;
                fadeImage.color = new Color(0, 0, 0, 1 - time / duration);
                yield return new WaitForSeconds(fadeDeltaTime);
            }
        }

        private IEnumerator FadeOutCoroutine(float duration)
        {
            float time = 0;
            while (time < duration)
            {
                time += fadeDeltaTime;
                fadeImage.color = new Color(0, 0, 0, time / duration);
                yield return new WaitForSeconds(fadeDeltaTime);
            }
        }
    }
}
