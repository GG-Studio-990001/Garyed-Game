using UnityEngine;

namespace Runtime.CH1.Main
{
    public class Ch1GameController : MonoBehaviour
    {
        [SerializeField] private GameObject gameSettingUI;
        [SerializeField] private SoundSystem soundSystem;
        
        GameOverControls _gameOverControls;

        private void Awake()
        {
            _gameOverControls = new GameOverControls();
        }
        
        private void OnEnable()
        {
            _gameOverControls.Enable();
            
            _gameOverControls.UI.GameSetting.performed += ctx => OnGameSetting();
        }
        
        private void OnDisable()
        {
            _gameOverControls.Disable();
        }
        
        private void Start()
        {
            SetAllUIActiveFalse();
            soundSystem.PlayMusic("Start");
        }
        
        private void SetAllUIActiveFalse()
        {
            gameSettingUI.SetActive(false);
        }
        
        private void OnGameSetting()
        {
            gameSettingUI.SetActive(gameSettingUI.activeSelf == false);
        }
    }
}