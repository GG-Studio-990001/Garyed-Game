using Runtime.CH1.Main.Player;
using Runtime.InGameSystem;
using UnityEngine;

namespace Runtime.CH1.Main
{
    public class Ch1GameController : MonoBehaviour
    {
        [Header("System")]
        [SerializeField] private SoundSystem soundSystem;
        
        private void Start()
        {
            InitGame();
        }
        
        private void InitGame()
        {
            SetMusic("Start");
        }
        
        private void SetMusic(string soundName)
        {
            soundSystem.StopMusic();
            soundSystem.PlayMusic(soundName);
        }

        public void RestrictPlayerInput()
        {
            DataProviderManager.Instance.ControlsDataProvider.Get().RestrictPlayerInput();
        }
        
        public void ReleasePlayerInput()
        {
            DataProviderManager.Instance.ControlsDataProvider.Get().ReleasePlayerInput();
        }
    }
}