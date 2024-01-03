using Runtime.CH1.Main.Player;
using Runtime.InGameSystem;
using UnityEngine;

namespace Runtime.CH1.Main
{
    public class Ch1GameController : MonoBehaviour
    {
        [Header("System")]
        [SerializeField] private SoundSystem soundSystem;

        public GameOverControls GameOverControls { get; private set; }
        
        private void Start()
        {
            InitGame();
        }
        
        private void InitGame()
        {
            //GameOverControls = DataProviderManager.Instance.ControlsDataProvider.Get().gameOverControls;
            
            SetMusic("Start");
        }
        
        private void SetMusic(string soundName)
        {
            soundSystem.StopMusic();
            soundSystem.PlayMusic(soundName);
        }
        
        #region Unity Event

        public void OnDialogueStart()
        {
            GameOverControls.Player.Disable();
        }
        
        public void OnDialogueEnd()
        {
            GameOverControls.Player.Enable();
        }

        #endregion
    }
}