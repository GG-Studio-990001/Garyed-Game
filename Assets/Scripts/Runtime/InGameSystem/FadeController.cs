using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.InGameSystem
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        // [SerializeField] private Image background;
        [SerializeField] private float fadeDeltaTime = 0.01f;

        private float FadeDuration { get; set; } = 1f;
        private Coroutine _currentFadeCoroutine;

        public void StartFadeIn()
        {
            StopCurrentFadeCoroutine();
            
            _currentFadeCoroutine = StartCoroutine(FadeInCoroutine(FadeDuration));
        }

        public void StartFadeOut()
        {
            StopCurrentFadeCoroutine();
            
            _currentFadeCoroutine = StartCoroutine(FadeOutCoroutine(FadeDuration));
        }

        //public void SetBackground(bool isBlack)
        //{
        //    StopCurrentFadeCoroutine();
        //    if (isBlack)
        //        background.color = Color.black;
        //    else
        //        background.color = Color.clear;
        //}

        private void StopCurrentFadeCoroutine()
        {
            if (_currentFadeCoroutine != null)
            {
                StopCoroutine(_currentFadeCoroutine);
                _currentFadeCoroutine = null;
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
