using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.InGameSystem
{
    public class FadeController : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDeltaTime = 0.01f;

        public float FadeDuration { get; set; } = 1f;
    
        public async Task FadeIn()
        {
             await FadeInCoroutine(FadeDuration);
        }
    
        public async Task FadeOut()
        {
            await FadeOutCoroutine(FadeDuration);
        }
    
        private async Task FadeInCoroutine(float duration)
        {
            float time = 0;
            while (time < duration)
            {
                time += fadeDeltaTime;
                fadeImage.color = new Color(0, 0, 0, 1 - time / duration);
                await Task.Delay((int)(fadeDeltaTime * 1000));
            }
        }
    
        private async Task FadeOutCoroutine(float duration)
        {
            float time = 0;
            while (time < duration)
            {
                time += fadeDeltaTime;
                fadeImage.color = new Color(0, 0, 0, time / duration);
                await Task.Delay((int)(fadeDeltaTime * 1000));
            }
        }
    }
}
