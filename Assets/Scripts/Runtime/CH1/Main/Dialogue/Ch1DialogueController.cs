using DG.Tweening;
using Runtime.ETC;
using Runtime.InGameSystem;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH1.Main.Dialogue
{
    // CH1 대화 컨트롤러 Yarn Spinner를 사용하여 대화를 관리하는 클래스
    public class Ch1DialogueController : DialogueViewBase
    {
        private CutSceneDialogue _cutScene;
        [Header("=Script=")]
        [SerializeField] private DialogueRunner _runner;
        [SerializeField] private SLGActionComponent SLGAction;
        [SerializeField] private NpcDialogueController _npcDialogue;
        [SerializeField] private FadeController _fadeController;
        // [SerializeField] private TimelineController _timelineController;
        // [SerializeField] private Image _backgroundImage;
        [Header("=Else=")]
        [SerializeField] private GameObject _nameTag;
        [SerializeField] private Volume _volume;
        private LowRes _lowRes;
        private string _speaker;

        public UnityEvent OnDialogueStart => _runner.onDialogueStart;
        public UnityEvent OnDialogueEnd => _runner.onDialogueComplete;

        private void Awake()
        {
            _cutScene = GetComponent<CutSceneDialogue>();
            _runner.AddCommandHandler("InitSLG", SLGAction.OnSLGInit);
            _runner.AddCommandHandler<int>("NpcDialogueFin", _npcDialogue.NpcDialogueFin);

            _runner.AddCommandHandler("FadeIn", _fadeController.StartFadeIn);
            _runner.AddCommandHandler("FadeOut", _fadeController.StartFadeOut);

            // _runner.AddCommandHandler<string>("StartTimeline", (timelineName) => _timelineController.PlayTimeline(timelineName));        
            _runner.AddCommandHandler<string>("SceneChange", SceneChange);
            _runner.AddCommandHandler("NewSceneStart", NewSceneStart);
            _runner.AddCommandHandler("NextSceneStart", NextSceneStart);
            _runner.AddCommandHandler("SceneEnd", SceneEnd);

            // CutScene
            _runner.AddCommandHandler<bool>("ShowIllustration", _cutScene.ShowIllustration);
            _runner.AddCommandHandler("PlayerInitPos", _cutScene.PlayerInitPos);
            _runner.AddCommandHandler<int>("CharactersMove", _cutScene.CharactersMove);
            _runner.AddCommandHandler<int>("CharactersStop", _cutScene.CharactersStop);
            _runner.AddCommandHandler<int>("NpcJump", _cutScene.NpcJump);
            _runner.AddCommandHandler("GetLucky", _cutScene.GetLucky);
            _runner.AddCommandHandler<bool>("ShakeMap", _cutScene.ShakeMap);
            _runner.AddCommandHandler("BreakBridge", _cutScene.BreakBridge);
            _runner.AddCommandHandler<int>("SetNpcPosition", _cutScene.SetNpcPosition);
            // CutScene / Mamago
            _runner.AddCommandHandler("ConstructionSFX", _cutScene.ConstructionSFX);
            _runner.AddCommandHandler("CompleteSFX", _cutScene.CompleteSFX);
            _runner.AddCommandHandler("MamagoJump", _cutScene.MamagoJump);
            _runner.AddCommandHandler("MamagoMove1", _cutScene.MamagoMove1);
            _runner.AddCommandHandler("MamagoMove2", _cutScene.MamagoMove2);
            _runner.AddCommandHandler("MamagoEnter", _cutScene.MamagoEnter);
            // CutScene / R2mon
            _runner.AddCommandHandler("SetR2MonPosition", _cutScene.SetR2MonPosition);
            _runner.AddCommandHandler("ChangeChapter2", _cutScene.ChangeChapter2);
            // _runner.AddCommandHandler<string>("ChangeScene", ChangeScene);

            if (_volume != null)
            {
                _volume.profile.TryGet(out _lowRes);
            }
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            _speaker = dialogueLine.CharacterName;
            SetNameTag(_speaker != "");

            onDialogueLineFinished();
        }

        private void SetNameTag(bool hasName)
        {
            _nameTag.SetActive(hasName);
        }

        private void Start()
        {
            if (Managers.Data.Scene == 0)
            {
                _runner.StartDialogue("S1");
            }
            else if (Managers.Data.Scene == 1 && Managers.Data.IsPacmomCleared)
            {
                _cutScene.NpcPos.SetNpcPosition(2);
                _cutScene.Player.transform.position = new Vector3(21.95f, -7.51f, 0);
                _runner.StartDialogue("S2");
            }
        }

        public void MamagoThanks()
        {
            if (Managers.Data.Scene == 4)
            {
                _runner.StartDialogue("S5");
            }
        }

        private void NewSceneStart()
        {
            Managers.Data.Scene++;
            Managers.Data.SceneDetail = 0;
            _cutScene.Player.IsDirecting = true;
        }

        private void NextSceneStart()
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
            Managers.Sound.StopBGM();
            Managers.Sound.Play(Sound.SFX, "[CH1] Pacmom_SFX_Connection");

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

        public void TextSFX()
        {
            if (_speaker == "R2-Mon")
            {
                Managers.Sound.Play(Sound.Speech, "R2MonText/[CH1] R2-Mon_Text_SFX_04");
            }
            else
            {
                Managers.Sound.Play(Sound.Speech, "[CH1] Text SFX");
            }
        }

        /*
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
