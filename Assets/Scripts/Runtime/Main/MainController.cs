using Runtime.InGameSystem;
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
            sceneSystem.LoadSceneWithFade("Title");
        }
        
        public void LoadGame()
        {
            Managers.Data.LoadGame();
            sceneSystem.LoadSceneWithFade("Title");
        }
    }
}
