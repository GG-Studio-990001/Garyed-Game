using NUnit.Framework;
using Runtime.Common.Domain;
using Runtime.Common.Presentation;
using Runtime.Common.View;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace Tests.Runtime
{
    [TestFixture]
    public class SettingsUITests
    {
        private GameObject _gameObject;
        private SettingsUIView _view;
        private SettingsData _model;
        private SettingsUIPresenter _presenter;
        
        private GameObject _backgroundAudioSource;
        private GameObject _effectAudioSource;
        
        private Slider _musicVolumeSlider;
        private Slider _sfxVolumeSlider;
        
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
            _view.SetMusicSlider(_musicVolumeSlider);
            
            _sfxVolumeSlider = new GameObject("SfxVolumeSlider").AddComponent<Slider>();
            _view.SetSfxSlider(_sfxVolumeSlider);
            
            _model = ScriptableObject.CreateInstance<SettingsData>();
            _presenter = new SettingsUIPresenter(_view, _model);
            
            _model.MusicVolume = 0.5f;
            _model.SfxVolume = 0.5f;
            
            yield return new WaitForFixedUpdate();
        }
        
        [UnityTest]
        public IEnumerator WhenMusicVolumeChangesViewIsUpdated()
        {
            _musicVolumeSlider.value = 0.75f;
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(0.75f, _musicVolumeSlider.value);
        }
        
        [UnityTest]
        public IEnumerator WhenSfxVolumeChangesViewIsUpdated()
        {
            _sfxVolumeSlider.value = 0.3f;
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(0.3f, _sfxVolumeSlider.value);
        }
        
        [UnityTest]
        public IEnumerator WhenMusicVolumeChangesModelIsUpdated()
        {
            _musicVolumeSlider.value = 0.75f;
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(0.75f, _model.MusicVolume);
        }
        
        [UnityTest]
        public IEnumerator WhenSfxVolumeChangesModelIsUpdated()
        {
            _musicVolumeSlider.value = 0.3f;
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(0.3f, _model.SfxVolume);
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(_gameObject);
            Object.DestroyImmediate(_backgroundAudioSource);
            Object.DestroyImmediate(_effectAudioSource);
            
            yield return new WaitForFixedUpdate();
        }
    }
}
