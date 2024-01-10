using Cinemachine;
using Runtime.CH1.Main.Map;
using Runtime.Data.Original;
using Runtime.InGameSystem;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.Main
{
    public class Ch1GameController : MonoBehaviour
    {
        [Header("System")]
        [SerializeField] private SoundSystem soundSystem;
        
        [Header("Player")]
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject[] stage;
        private GameObject _currentStage;
        
        [Header("Camera")]
        [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;
        
        [Header("Cursor")]
        [SerializeField] private Texture2D cursorTexture;
        
        private IProvider<PlayerData> _playerDataProvider;
        
        private void Start()
        {
            _playerDataProvider = DataProviderManager.Instance.PlayerDataProvider;
            
            InitGame();
                
            var data = _playerDataProvider.Get();
            NextStage(data.quarter.stage, data.position);
            
            //Test
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        }
        
        private void InitGame()
        {
            SetMusic("Start");
            //SoundManager.Instance.PlaySound("Start");
        }
        
        private void SetMusic(string soundName)
        {
            soundSystem.StopMusic();
            soundSystem.PlayMusic(soundName);
        }
        
        // TODO 메서드 정리
        // 이 메서드가 시작되면 Fade시작해서 종료되면 Fade 종료
        public void NextStage(int stageNumber, Vector2 playerPosition)
        {
            if (_currentStage != null)
                _currentStage.SetActive(false);
            _currentStage = stage[stageNumber - 1];

            var stageComponent = _currentStage.GetComponent<Stage>();
            
            stageComponent.Ch1GameController = this;
            stageComponent.CinemachineConfiner2D = cinemachineConfiner2D;
            
            stageComponent.SetMapSetting();

            var data = _playerDataProvider.Get();
            player.transform.position = playerPosition;
            data.position = playerPosition;
            data.quarter.stage = stageNumber;
            
            _playerDataProvider.Set(data);
            
            _currentStage.SetActive(true);
        }
    }
}