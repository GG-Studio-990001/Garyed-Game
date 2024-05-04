using Runtime.InGameSystem;
using System;
using UnityEngine;

namespace Runtime.Main
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private SceneSystem sceneSystem;

        private void Start()
        {
            Managers.Data.InGameKeyBinder.GameControlReset();
        }

        public void NewGame()
        {
            Managers.Data.NewGame();
            sceneSystem.LoadSceneWithFade($"CH{Managers.Data.Chapter}");
        }
        
        public void LoadGame()
        {
            Managers.Data.LoadGame();
            sceneSystem.LoadSceneWithFade($"CH{Managers.Data.Chapter}");
        }
    }
}
