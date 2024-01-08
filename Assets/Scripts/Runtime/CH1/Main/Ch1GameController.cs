using Cinemachine;
using Runtime.CH1.Main.Map;
using Runtime.CH1.Main.Player;
using Runtime.Data.Original;
using Runtime.InGameSystem;
using Runtime.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
        
        private IProvider<PlayerData> _playerDataProvider;
        
        private void Start()
        {
            _playerDataProvider = DataProviderManager.Instance.PlayerDataProvider;
            
            InitGame();
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
        public void NextStage(int stageNumber)
        {
            if (_currentStage != null)
                _currentStage.SetActive(false);
            _currentStage = stage[stageNumber - 1];

            var stageComponent = _currentStage.GetComponent<Stage>();
            
            stageComponent.Ch1GameController = this;
            stageComponent.CinemachineConfiner2D = cinemachineConfiner2D;
            
            stageComponent.SetMapSetting();

            var data = _playerDataProvider.Get();
            player.transform.position = data.position;
            data.quarter.stage = stageNumber;
            
            _playerDataProvider.Set(data);
            
            _currentStage.SetActive(true);
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