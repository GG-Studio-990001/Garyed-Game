using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Runtime.InGameSystem
{
    public class SceneSystem : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            DataProviderManager.Instance.ControlsDataProvider.Get().ResetControls();
            SceneManager.LoadScene(sceneName);
        }
    }
}
