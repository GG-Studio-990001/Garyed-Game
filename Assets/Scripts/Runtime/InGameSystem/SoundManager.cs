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

                //InitializeSounds("Sound");
                return;
            }
            
            Destroy(gameObject);
        }

        private async void Start()
        {
            Debug.Log("Start");
            // Addressable 리소스의 위치 정보 비동기 로딩
            AsyncOperationHandle<IList<IResourceLocation>> operationHandle = Addressables.LoadResourceLocationsAsync("Sound");

            // 비동기 작업 완료 대기
            await operationHandle.Task;
            Debug.Log("Wait");

            // 위치 정보 획득
            IList<IResourceLocation> locations = operationHandle.Result;

            // 위치 정보를 사용하여 다양한 작업 수행
            foreach (var location in locations)
            {
                // 위치 정보를 이용한 작업 수행
                Debug.Log("Resource Location: " + location.PrimaryKey);
            }

            // 핸들 해제
            Addressables.Release(operationHandle);
            
            Debug.Log("End");
        }

        private void InitializeSounds(string groupName)
        {
            Debug.Log("??");
            Addressables.LoadResourceLocationsAsync(groupName).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("??>");
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