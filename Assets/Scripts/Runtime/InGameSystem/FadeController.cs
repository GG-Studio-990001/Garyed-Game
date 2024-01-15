using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.InGameSystem
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDeltaTime = 0.01f;
    
        public void FadeIn(float duration)
        {
            StartCoroutine(FadeInCoroutine(duration));
        }
    
        public void FadeOut(float duration)
        {
            StartCoroutine(FadeOutCoroutine(duration));
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
