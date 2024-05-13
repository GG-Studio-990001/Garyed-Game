using Cinemachine;
using DG.Tweening;
using Runtime.InGameSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using Sound = Runtime.ETC.Sound;

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
        
        [SerializeField] private Volume _volume;
        private LowRes _lowRes;
        
        // public List<Sprite> Sprites = new List<Sprite>();

        public UnityEvent OnDialogueStart => _runner.onDialogueStart;
        public UnityEvent OnDialogueEnd => _runner.onDialogueComplete;

        [SerializeField] private CutSceneDialogue _cutScene;

        private void Awake()
        {
            _runner.AddCommandHandler<string>("StartTimeline", (timelineName) => _timelineController.PlayTimeline(timelineName));
            _runner.AddCommandHandler<string>("SceneChange", SceneChange);
            _runner.AddCommandHandler("FadeOut", _fadeController.StartFadeOut);
            _runner.AddCommandHandler("FadeIn", _fadeController.StartFadeIn);

            // CutScene
            _runner.AddCommandHandler("NewSceneStart", NewSceneStart);
            _runner.AddCommandHandler("SceneStart", SceneStart);
            _runner.AddCommandHandler("SceneEnd", SceneEnd);
            _runner.AddCommandHandler<int>("ShowIllustration", _cutScene.ShowIllustration);
            _runner.AddCommandHandler("HideIllustration", _cutScene.HideIllustration);
            _runner.AddCommandHandler("CharactersMove", _cutScene.CharactersMove1);
            _runner.AddCommandHandler("CharactersStop", _cutScene.CharactersStop1);
            _runner.AddCommandHandler<int>("NpcJump", _cutScene.NpcJump);

            _runner.AddCommandHandler("GetLucky", _cutScene.GetLucky);

            /*
            // UI/Sound
            _runner.AddCommandHandler<string>("PlayBackgroundSound", PlayBackgroundSound);
            _runner.AddCommandHandler<bool>("SetBackgroundColor", SetBackgroundColor);
            _runner.AddCommandHandler<string>("ChangeScene", ChangeScene);
            _runner.AddCommandHandler("SetCamera", SetCamera);
            _runner.AddCommandHandler("CurrentMinorDialogueStart", CurrentMinorDialogueStart);
            */
            _runner.AddCommandHandler("SLGSetting", SetSLGUI);
            
            if (_volume != null)
            {
                _volume.profile.TryGet(out _lowRes);
            }
        }

        private void Start()
        {
            if (Managers.Data.Scene == 0 && Managers.Data.SceneDetail == 0)
                _runner.StartDialogue("S1");
        }

        public void CheckCutScene()
        {
            if (Managers.Data.Scene == 1 && Managers.Data.SceneDetail == 1)
            {
                _runner.StartDialogue("S1.2");
                //_runner.StartDialogue($"Dialogue{Managers.Data.Minor}");
            }
        }
        private void NewSceneStart()
        {
            Managers.Data.Scene++;
        }

        private void SceneStart()
        {
            Managers.Data.SceneDetail++;
            _cutScene.Player.IsDirecting = true;
        }

        private void SceneEnd()
        {
            _cutScene.Player.IsDirecting = false;
            Managers.Data.SaveGame();
            Debug.Log(Managers.Data.Scene + " " + Managers.Data.SceneDetail);
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

        public void TypingSFX()
        {
            Managers.Sound.Play(Sound.SFX, "[CH1] Text SFX");
        }

        private void SetSLGUI()
        {

        }
        /*        
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
