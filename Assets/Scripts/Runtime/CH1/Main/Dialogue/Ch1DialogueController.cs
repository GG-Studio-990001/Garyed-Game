using Cinemachine;
using Runtime.Data.Original;
using Runtime.InGameSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Yarn.Unity;

namespace Runtime.CH1.Main.Dialogue
{
    public class Ch1DialogueController : DialogueViewBase
    {
        [SerializeField] private DialogueRunner _runner;
        
        [SerializeField] private SoundSystem _soundSystem;
        [SerializeField] private FadeController _fadeController;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private TimelineController _timelineController;
        [SerializeField] private Image _backgroundImage;
        
        public List<Sprite> Sprites = new List<Sprite>();

        public UnityEvent OnDialogueStart => _runner.onDialogueStart;
        public UnityEvent OnDialogueEnd => _runner.onDialogueComplete;
        
        //public PlayerData playerData;
        
        private void Awake()
        {
            // UI/Sound
            _runner.AddCommandHandler<string>("PlayBackgroundSound", PlayBackgroundSound);
            //_runner.AddCommandHandler("StopBackgroundSound", _soundSystem.StopMusic);
            _runner.AddCommandHandler<bool>("SetBackgroundColor", SetBackgroundColor);
            _runner.AddCommandHandler("FadeOut", _fadeController.StartFadeOut);
            _runner.AddCommandHandler("FadeIn", _fadeController.StartFadeIn);
            _runner.AddCommandHandler<string>("ChangeScene", ChangeScene);
            
            // Camera
            _runner.AddCommandHandler("SetCamera", SetCamera);

            // Logic
            _runner.AddCommandHandler("CurrentMinorDialogueStart", CurrentMinorDialogueStart);
            //_runner.AddCommandHandler("MinorVersionUp", () => playerData.quarter.minor++);
            _runner.AddCommandHandler<string>("StartTimeline", (timelineName) => _timelineController.PlayTimeline(timelineName));
            
            // Character
        }
        
        private void PlayBackgroundSound(string soundName)
        {
            _soundSystem.PlayMusic(soundName);
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
            _runner.Stop();
            //_runner.Clear();
            //_runner.StartDialogue($"Dialogue{playerData.quarter.minor}");
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
        
        public void SetDialogueData(string value)
        {
            var variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();

            if (value == "ThreeMatchPuzzle")
            {
                //variableStorage.TryGetValue("$ThreeMatchPuzzle", out lvalue);
                variableStorage.SetValue("$ThreeMatchPuzzle", true);
            }
        }
    }
}
