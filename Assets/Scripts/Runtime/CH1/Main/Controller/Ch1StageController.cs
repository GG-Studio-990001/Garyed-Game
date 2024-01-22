using Cinemachine;
using Runtime.CH1.Main.Stage;
using Runtime.InGameSystem;
using Runtime.Interface;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Main.Controller
{
    public class Ch1StageController : MonoBehaviour, IStageController
    {
        public IStage CurrentStage { get; set; }
        public IStageChanger StageChanger { get; set; }
        public CinemachineConfiner2D Confiner2D { get; private set; }
        
        private IStage[] _stages;
        private GameObject _player;
        private Ch1MainSystemController _mainSystemController;
        private FadeController _fadeController;
        
        public void Init(GameObject player, CinemachineConfiner2D confiner2D, FadeController fadeController, Ch1MainSystemController mainSystemController)
        {
            _player = player;
            Confiner2D = confiner2D;
            _fadeController = fadeController;
            _mainSystemController = mainSystemController;
            
            _stages = GetComponentsInChildren<IStage>();
            
            foreach (var stage in _stages)
            {
                stage.StageController = this;
            }
            
            // 개발용: 인스펙터에서 스테이지를 바꿀 수 있도록 하기 위해
            foreach (var stage in _stages)
            {
                if (stage.IsActivate())
                {
                    CurrentStage = stage;
                    CurrentStage.SetSetting();
                }
            }
            
            if (_fadeController is not null)
                _fadeController.FadeIn();
            
            StagerChangerInit();
        }
        
        private void StagerChangerInit()
        {
            StageChanger = new Ch1StageChanger(_player, CurrentStage, _stages, _fadeController);
            
            StageChanger.OnStageStart += () => _mainSystemController.PlayerInputLimit(true);
            StageChanger.OnStageEnd += () => _mainSystemController.PlayerInputLimit(false);
        }
    }
}
