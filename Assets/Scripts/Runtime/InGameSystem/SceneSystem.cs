using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.InGameSystem
{
    public class SceneSystem : MonoBehaviour
    {
        [SerializeField] private FadeController fadeController;
        
        public void LoadScene(string sceneName)
        {
            Managers.Sound.StopBGM();
            SceneManager.LoadScene(sceneName);
        }
        
        public void LoadSceneWithFade(string sceneName)
        {
            if (fadeController == null)
            {
                LoadScene(sceneName);
            }
            else
            {
                StartCoroutine(CoroutineLoadSceneWithFade(sceneName));
            }
        }
        
        private IEnumerator CoroutineLoadSceneWithFade(string sceneName)
        {
            fadeController.StartFadeOut();
            
            yield return new WaitForSeconds(1);
            
            LoadScene(sceneName);
        }

        public void ExitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
