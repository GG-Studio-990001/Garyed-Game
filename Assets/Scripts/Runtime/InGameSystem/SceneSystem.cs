using System;
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
            DataProviderManager.Instance.ControlsDataProvider.Get().ResetControls();
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
            fadeController.FadeOut(1);
            
            yield return new WaitForSeconds(1);
            
            LoadScene(sceneName);
        }
    }
}
