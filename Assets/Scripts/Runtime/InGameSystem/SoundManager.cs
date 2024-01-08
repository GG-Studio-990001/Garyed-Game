using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Runtime.InGameSystem
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        private Dictionary<string, AssetReference> _soundReferences = new Dictionary<string, AssetReference>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);

                InitializeSounds("Sound");
                PlaySound("BGM_Mamago");
                return;
            }
            
            Destroy(gameObject);
        }
        
        private void InitializeSounds(string groupName)
        {
            Addressables.LoadResourceLocationsAsync(groupName).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    foreach (var location in handle.Result)
                    {
                        if (location.ResourceType == typeof(AudioClip))
                        {
                            Debug.Log($"Load sound {location.PrimaryKey}");
                            string soundName = location.PrimaryKey;
                            _soundReferences.Add(soundName, new AssetReference(soundName));
                        }
                    }
                }
                else
                {
                    Debug.LogError($"Cannot load sound group {groupName}");
                }
            };
        }

        public void PlaySound(string soundName)
        {
            if (_soundReferences.TryGetValue(soundName, out var soundReference))
            {
                soundReference.LoadAssetAsync<AudioClip>().Completed += handle =>
                {
                    if (handle.Status == AsyncOperationStatus.Succeeded)
                    {
                        AudioSource.PlayClipAtPoint(handle.Result, Vector3.zero);
                    }
                    else
                    {
                        Debug.LogError($"Cannot load sound {soundName}");
                    }
                };
            }
            else
            {
                Debug.LogError($"Cannot find sound {soundName}");
            }
        }
    }
}