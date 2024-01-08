using Runtime.Data.Original;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.Data.Provider
{
    public class SettingsDataProvider : IProvider<SettingsData>
    {
        private readonly SettingsData _settingsData;
        
        public SettingsDataProvider(SettingsData settingsData)
        {
            _settingsData = settingsData;
        }
        
        public SettingsData Get()
        {
            SettingsData settingsData = ScriptableObject.CreateInstance<SettingsData>();
            settingsData.MusicVolume = _settingsData.MusicVolume;
            settingsData.SfxVolume = _settingsData.SfxVolume;
            
            return settingsData;
        }

        public void Set(SettingsData value)
        {
            _settingsData.MusicVolume = value.MusicVolume;
            _settingsData.SfxVolume = value.SfxVolume;
        }
    }
}