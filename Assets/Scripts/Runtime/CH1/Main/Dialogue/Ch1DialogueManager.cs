using Cinemachine;
using Runtime.InGameSystem;
using System;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Main.Dialogue
{
    public class Ch1DialogueManager : DialogueViewBase
    {
        [SerializeField] private DialogueRunner _runner;
        
        [Header("System")]
        [SerializeField] private SoundSystem _soundSystem;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        private void Awake()
        {
            //_runner.AddCommandHandler("PlaySound", PlaySound);
            _runner.AddCommandHandler("SetCamera", SetCamera);
        }
        
        private int PlaySound()
        {
            return 1;
        }
        
        private void SetCamera()
        {
            _virtualCamera.m_Lens.FieldOfView = 30;
        }
    }
}
