using Cinemachine;
using DG.Tweening;
using Runtime.InGameSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;

namespace Runtime.CH1.Main.Dialogue
{
    // CH1 대화 컨트롤러 Yarn Spinner를 사용하여 대화를 관리하는 클래스
    public class Ch1DialogueController : DialogueViewBase
    {
        [SerializeField] private DialogueRunner _runner;
        [SerializeField] private FadeController _fadeController;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private TimelineController _timelineController;
        [SerializeField] private Image _backgroundImage;
        
        [SerializeField] private GameObject _slgUI;
        [SerializeField] private Volume _volume;
        private LowRes _lowRes;
        
        public List<Sprite> Sprites = new List<Sprite>();

        public UnityEvent OnDialogueStart => _runner.onDialogueStart;
        public UnityEvent OnDialogueEnd => _runner.onDialogueComplete;

        [Header("=CutScene=")]
        [SerializeField] private GameObject illerstrationParent;
        [SerializeField] private GameObject[] illerstration = new GameObject[3];
        [SerializeField] private GameObject[] npcs = new GameObject[3];
        [SerializeField] private Vector3[] locations = new Vector3[3];
        [SerializeField] private GameObject lucky;

        private void Awake()
        {
            _runner.AddCommandHandler<string>("StartTimeline", (timelineName) => _timelineController.PlayTimeline(timelineName));
            _runner.AddCommandHandler<string>("SceneChange", SceneChange);
            _runner.AddCommandHandler("FadeOut", _fadeController.StartFadeOut);
            _runner.AddCommandHandler("FadeIn", _fadeController.StartFadeIn);

            // CutScene
            _runner.AddCommandHandler<int>("ShowIllustration", ShowIllustration);
            _runner.AddCommandHandler("HideIllustration", HideIllustration);
            _runner.AddCommandHandler("CharactersMove", CharactersMove);

            _runner.AddCommandHandler("GetLucky", GetLucky);

            /*
            // UI/Sound
            _runner.AddCommandHandler<string>("PlayBackgroundSound", PlayBackgroundSound);
            _runner.AddCommandHandler<bool>("SetBackgroundColor", SetBackgroundColor);
            _runner.AddCommandHandler<string>("ChangeScene", ChangeScene);
            _runner.AddCommandHandler("SetCamera", SetCamera);
            _runner.AddCommandHandler("CurrentMinorDialogueStart", CurrentMinorDialogueStart);
            _runner.AddCommandHandler("SLGSetting", SetSLUUI);
            */
            if (_volume != null)
            {
                _volume.profile.TryGet(out _lowRes);
            }
        }

        private void GetLucky()
        {
            lucky.SetActive(false);
        }

        private void ShowIllustration(int num)
        {
            illerstrationParent.SetActive(true);

            for (int i=0; i<3; i++)
            {
                if (i == num)
                    illerstration[i].SetActive(true);
                else
                    illerstration[i].SetActive(false);
            }
        }

        private void HideIllustration()
        {
            illerstrationParent.SetActive(false);
        }

        private void CharactersMove()
        {
            for (int i = 0; i < 4; i++)
            {
                npcs[i].transform.DOMove(locations[i], 5f);
            }
        }

        private void SceneChange(string sceneName)
        {
            // Hard Coding
            if ("Pacmom" == sceneName)
            {
                float startValue = 500f;
                float endValue = -75f;
                float duration = 2f;

                _lowRes.IsActive();

                Managers.Data.InGameKeyBinder.PlayerInputDisable();

                DOVirtual.Float(startValue, endValue, duration, currentValue =>
                {
                    _lowRes.height.value = (int)currentValue;
                }).SetEase(Ease.Linear).onComplete += () =>
                {
                    Managers.Data.InGameKeyBinder.PlayerInputEnable();
                    Managers.Sound.StopAllSound();
                    SceneManager.LoadScene("Pacmom");
                };
            }
        }

        public void SetDialogueData(string value)
        {
            var variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();

            if (value == "ThreeMatchPuzzle")
            {
                //variableStorage.TryGetValue("$ThreeMatchPuzzle", out lvalue);
                variableStorage.SetValue("$ThreeMatchPuzzle", true);
            }
        }

        /*
        private void SetSLUUI()
        {
            _slgUI.SetActive(true);
            //TODO 함수 구조 고민
            SLGActionComponent SLGAction = FindObjectOfType<SLGActionComponent>();
            if(SLGAction != null)
            {
                SLGAction.OnSLGInit();
            }
        }
        
        private void PlayBackgroundSound(string soundName)
        {
            //_soundSystem.PlayMusic(soundName); // TODO Manager.Sound로 교체
        }

        private void SetCamera()
        {
            _virtualCamera.m_Lens.FieldOfView = 30; // ?
        }
        
        private void SetBackgroundColor(bool isBlack)
        {
            _fadeController.SetBackground(isBlack);
        }
        
        public void CurrentMinorDialogueStart()
        {
            //_runner.NodeExists();
           // _runner.Stop();
            //_runner.Clear();
            _runner.StartDialogue($"Dialogue{Managers.Data.Minor}");
        }
        
        private void ChangeScene(string spriteName)
        {
            if (spriteName == "None")
            {
                _backgroundImage.color = Color.clear;
                _backgroundImage.sprite = null;
                return;
            }
            
            _backgroundImage.color = Color.white;
            
            Sprite sprite = Fetch<Sprite>(spriteName);
            if (sprite != null)
            {
                _backgroundImage.sprite = sprite;
            }
        }
        
        T Fetch<T>(string spriteName) where T : UnityEngine.Object
        {
            if (typeof(T) == typeof(Sprite))
            {
                foreach (var sprite in Sprites)
                {
                    if (sprite.name == spriteName)
                    {
                        return sprite as T;
                    }
                }
            }

            return null;
        }
        */
    }
}
