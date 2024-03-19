using NUnit.Framework;
using Runtime.Common.Presentation;
using Runtime.Common.View;
using Runtime.Data.Original;
using Runtime.InGameSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.Runtime.System
{
    [TestFixture]
    public class SettingsUITests
    {
        private GameObject _gameObject;
        private SettingsUIView _view;
        private SettingsUIPresenter _presenter;
        
        private GameObject _backgroundAudioSource;
        private GameObject _effectAudioSource;
        
        private Slider _musicVolumeSlider;
        private Slider _sfxVolumeSlider;
        private Button _gameExitButton;
        private Button _exitButton;
        
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _backgroundAudioSource = new GameObject("BackgroundAudioSource");
            _backgroundAudioSource.AddComponent<AudioSource>();
            
            _effectAudioSource = new GameObject("EffectAudioSource");
            _effectAudioSource.AddComponent<AudioSource>();
            
            _gameObject = new GameObject("SettingsUI");
            _view = _gameObject.AddComponent<SettingsUIView>();

            _musicVolumeSlider = new GameObject("MusicVolumeSlider").AddComponent<Slider>();
            _view.MusicVolumeSlider = _musicVolumeSlider;
            
            _sfxVolumeSlider = new GameObject("SfxVolumeSlider").AddComponent<Slider>();
            _view.SfxVolumeSlider = _sfxVolumeSlider;
            
            _gameExitButton = new GameObject("GameExitButton").AddComponent<Button>();
            _view.GameExitButton = _gameExitButton;
            
            _exitButton = new GameObject("ExitButton").AddComponent<Button>();
            _view.ExitButton = _exitButton;
            
            _presenter = new SettingsUIPresenter(_view);
            
            yield return new WaitForFixedUpdate();
        }
        
        [UnityTest]
        public IEnumerator WhenMusicVolumeChangesViewIsUpdated()
        {
            // Arrange
            _musicVolumeSlider.value = 0.75f;
            
            // Act
            yield return new WaitForFixedUpdate();
            
            // Assert
            Assert.AreEqual(0.75f, _musicVolumeSlider.value);
        }
        
        [UnityTest]
        public IEnumerator WhenSfxVolumeChangesViewIsUpdated()
        {
            // Arrange
            _sfxVolumeSlider.value = 0.3f;
            
            // Act
            yield return new WaitForFixedUpdate();
            
            // Assert
            Assert.AreEqual(0.3f, _sfxVolumeSlider.value);
        }
        
        [UnityTest]
        public IEnumerator WhenMusicVolumeChangesModelIsUpdated()
        {
            // Arrange
            _musicVolumeSlider.value = 0.75f;
            
            // Act
            yield return new WaitForFixedUpdate();
            
            // Assert
        }
        
        [UnityTest]
        public IEnumerator WhenSfxVolumeChangesModelIsUpdated()
        {
            // Arrange
            _sfxVolumeSlider.value = 0.3f;
            
            // Act
            yield return new WaitForFixedUpdate();
            
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(_gameObject);
            Object.DestroyImmediate(_backgroundAudioSource);
            Object.DestroyImmediate(_effectAudioSource);
            Object.DestroyImmediate(_musicVolumeSlider.gameObject);
            Object.DestroyImmediate(_sfxVolumeSlider.gameObject);
            Object.DestroyImmediate(_exitButton.gameObject);
            Object.DestroyImmediate(_gameExitButton.gameObject);
            
            yield return new WaitForFixedUpdate();
        }
    }
}
