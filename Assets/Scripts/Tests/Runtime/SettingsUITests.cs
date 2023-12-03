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
        
        [UnitySetUp]
        public IEnumerator SetUp()
        {
            _gameObject = new GameObject("SettingsUI");
            _view = _gameObject.AddComponent<SettingsUIView>();
            
            _view.musicVolumeSlider = new GameObject("MusicVolumeSlider").AddComponent<Slider>();
            _view.sfxVolumeSlider = new GameObject("SfxVolumeSlider").AddComponent<Slider>();
            
            GameObject backgroundAudioSource = new GameObject("BackgroundAudioSource");
            backgroundAudioSource.AddComponent<AudioSource>();
            
            GameObject effectAudioSource = new GameObject("EffectAudioSource");
            effectAudioSource.AddComponent<AudioSource>();
            
            _model = ScriptableObject.CreateInstance<SettingsData>();
            _presenter = new SettingsUIPresenter(_view, _model);
            
            _model.MusicVolume = 0.5f;
            _model.SfxVolume = 0.5f;
            
            yield return new WaitForFixedUpdate();
        }
        
        [UnityTest]
        public IEnumerator WhenMusicVolumeChangesViewIsUpdated()
        {
            _presenter.SetMusicVolume(0.75f);
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(0.75f, _view.GetViewMusicVolume());
        }
        
        [UnityTest]
        public IEnumerator WhenSfxVolumeChangesViewIsUpdated()
        {
            _presenter.SetSfxVolume(0.3f);
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(0.3f, _view.GetViewSfxVolume());
        }
        
        [UnityTest]
        public IEnumerator WhenMusicVolumeChangesModelIsUpdated()
        {
            _presenter.SetMusicVolume(0.75f);
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(0.75f, _model.MusicVolume);
        }
        
        [UnityTest]
        public IEnumerator WhenSfxVolumeChangesModelIsUpdated()
        {
            _presenter.SetSfxVolume(0.3f);
            
            yield return new WaitForFixedUpdate();
            
            Assert.AreEqual(0.3f, _model.SfxVolume);
        }
        
        [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.DestroyImmediate(_gameObject);
            
            yield return new WaitForFixedUpdate();
        }
    }
}
