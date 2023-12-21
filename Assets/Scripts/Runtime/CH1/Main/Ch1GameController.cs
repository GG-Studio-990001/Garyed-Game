using UnityEngine;

namespace Runtime.CH1.Main
{
    public class Ch1GameSystem : MonoBehaviour
    {
        [SerializeField] private GameObject gameSettingUI;
        [SerializeField] private SoundSystem soundSystem;

        public GameOverControls GameOverControls { get; private set; }

        #region Singleton

        private static Ch1GameSystem _instance = null;
        
        public static Ch1GameSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType(typeof(Ch1GameSystem)) as Ch1GameSystem;
                    if (_instance == null)
                    {
                        Debug.LogError("There's no active Ch1GameSystem object");
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                _instance = this;
            }
            else if (Instance != this)
            {
                Destroy(this.gameObject);
            }

            InitGame();
        }

        #endregion
        
        private void InitGame()
        {
            GameOverControls = new GameOverControls();
            
            SetAllUIActiveFalse();

            soundSystem.PlayMusic("Start");
                
            GameOverControls.Enable();
            
            GameOverControls.UI.GameSetting.performed += ctx => OnGameSetting();
        }
        
        private void SetAllUIActiveFalse()
        {
            gameSettingUI.SetActive(false);
        }
        
        private void OnGameSetting()
        {
            gameSettingUI.SetActive(gameSettingUI.activeSelf == false);
        }
        
        public void OnDialogueStart()
        {
            GameOverControls.Player.Disable();
        }
        
        public void OnDialogueEnd()
        {
            GameOverControls.Player.Enable();
        }
        
        public void OnGameSettingStart()
        {
            GameOverControls.Player.Disable();
        }
        
        public void OnGameSettingEnd()
        {
            GameOverControls.Player.Enable();
        }
    }
}