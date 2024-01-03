using Runtime.Data;
using Runtime.Interface;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.InGameSystem
{
    public class DataProviderManager : MonoBehaviour
    {
        public static DataProviderManager Instance { get; private set; }
        public IProvider<PlayerData> PlayerDataProvider { get; private set; }
        public IProvider<SettingsData> SettingsDataProvider { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);

                Init();
                
                return;
            }

            Destroy(gameObject);
        }
        
        private void Init()
        {
            PlayerDataProvider = Addressables.LoadAssetAsync<PlayerDataProvider>("PlayerData").WaitForCompletion();
            SettingsDataProvider = Addressables.LoadAssetAsync<SettingsDataProvider>("SettingsData").WaitForCompletion();
        }
    }
}