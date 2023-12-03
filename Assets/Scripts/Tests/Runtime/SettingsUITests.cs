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
            
            _model = ScriptableObject.CreateInstance<SettingsData>();
            _presenter = new SettingsUIPresenter(_view, _model);
            
            _model.MusicVolume = 0.5f;
            _model.SfxVolume = 0.5f;
            
            yield return new WaitForFixedUpdate();
        }
        
        [Test]
        public void CheckInitialValue()
        {
            Assert.AreEqual(0.5f, _model.MusicVolume);
            Assert.AreEqual(0.5f, _model.SfxVolume);
        }
        
        [Test]
        public void WhenMusicVolumeChangesViewIsUpdated()
        {
            _presenter.SetMusicVolume(0.75f);
            Assert.AreEqual(0.75f, _view.GetViewMusicVolume());
        }
        
        [Test]
        public void WhenSfxVolumeChangesViewIsUpdated()
        {
            _presenter.SetSfxVolume(0.3f);
            Assert.AreEqual(0.3f, _view.GetViewSfxVolume());
        }
        
        [Test]
        public void WhenMusicVolumeChangesModelIsUpdated()
        {
            _presenter.SetMusicVolume(0.75f);
            Assert.AreEqual(0.75f, _model.MusicVolume);
        }
        
        [Test]
        public void WhenSfxVolumeChangesModelIsUpdated()
        {
            _presenter.SetSfxVolume(0.3f);
            Assert.AreEqual(0.3f, _model.SfxVolume);
        }
        
        [UnityTearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_gameObject);
        }
    }
}
