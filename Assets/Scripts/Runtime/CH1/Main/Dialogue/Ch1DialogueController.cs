using Cinemachine;
using DG.Tweening;
using Runtime.CH1.Main.Player;
using Runtime.ETC;
using Runtime.InGameSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarn.Unity;
using static UnityEditor.Experimental.GraphView.GraphView;
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
        
        [SerializeField] private GameObject _slgUI;
        [SerializeField] private Volume _volume;
        private LowRes _lowRes;
        
        // public List<Sprite> Sprites = new List<Sprite>();

        public UnityEvent OnDialogueStart => _runner.onDialogueStart;
        public UnityEvent OnDialogueEnd => _runner.onDialogueComplete;

        [Header("=CutScene=")]
        [SerializeField] private GameObject _illerstrationParent;
        [SerializeField] private GameObject[] _illerstration = new GameObject[1];
        [SerializeField] private GameObject _lucky;
        [Header("=Player=")]
        [SerializeField] private TopDownPlayer _player;
        [SerializeField] private Vector3 _location;
        [Header("=Npc=")]
        [SerializeField] private Npc[] _npc = new Npc[3];
        [SerializeField] private Vector3[] _locations = new Vector3[3];

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
            _runner.AddCommandHandler<int>("ShowIllustration", ShowIllustration);
            _runner.AddCommandHandler("HideIllustration", HideIllustration);
            _runner.AddCommandHandler("CharactersMove", CharactersMove);
            _runner.AddCommandHandler("CharactersStop", CharactersStop);

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

        private void NewSceneStart()
        {
            Managers.Data.Scene++;
        }

        private void SceneStart()
        {
            Managers.Data.SceneDetail++;
            _player.IsDirecting = true;
            
        }

        private void SceneEnd()
        {
            _player.IsDirecting = false;
            Managers.Data.SaveGame();
            Debug.Log(Managers.Data.Scene + " " + Managers.Data.SceneDetail);
        }    

        public void CheckCutScene()
        {
            if (Managers.Data.Scene == 1 && Managers.Data.SceneDetail == 1)
            {
                _runner.StartDialogue("S1.2");
                //_runner.StartDialogue($"Dialogue{Managers.Data.Minor}");
            }
        }

        private void GetLucky()
        {
            _lucky.SetActive(false);
        }

        private void ShowIllustration(int num)
        {
            _illerstrationParent.SetActive(true);

            for (int i=0; i< _illerstration.Length; i++)
            {
                if (i == num)
                    _illerstration[i].SetActive(true);
                else
                    _illerstration[i].SetActive(false);
            }
        }

        private void HideIllustration()
        {
            _illerstrationParent.SetActive(false);
        }

        private void CharactersMove()
        {
            string state = PlayerState.Move.ToString();

            _player._animation.SetAnimation(state, Vector2.right);
            _player.transform.DOMove(_location, 5f).SetEase(Ease.Linear);

            for (int i = 0; i < _npc.Length; i++)
            {
                _npc[i].Anim.SetAnimation(state, Vector2.right);
                _npc[i].transform.DOMove(_locations[i], 5f).SetEase(Ease.Linear);
            }
        }

        private void CharactersStop()
        {
            string state = PlayerState.Idle.ToString();

            _player._animation.SetAnimation(state, Vector2.down);

            _npc[0].Anim.SetAnimation(state, Vector2.right);
            _npc[1].Anim.SetAnimation(state, Vector2.up);
            _npc[2].Anim.SetAnimation(state, Vector2.left);
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
