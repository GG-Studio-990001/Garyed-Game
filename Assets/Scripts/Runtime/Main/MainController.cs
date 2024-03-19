using Runtime.InGameSystem;
using UnityEngine;

namespace Runtime.Main
{
    public class MainController : MonoBehaviour
    {
        [SerializeField] private SceneSystem sceneSystem;
        
        public void NewGame()
        {
            sceneSystem.LoadSceneWithFade($"CH{Managers.Data.Chapter}");
        }
        
        public void LoadGame()
        {
            Managers.Data.LoadGame();
            sceneSystem.LoadSceneWithFade($"CH{Managers.Data.Chapter}");
        }
    }
}
