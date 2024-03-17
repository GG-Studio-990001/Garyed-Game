using Runtime.ETC;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.InGameSystem
{
    // 코드에서 SoundType은 정적으로 3개로 고정이기 때문에
    // 구분하지 않고 분기문으로 처리, Sound 타입에 따라 정리
    public class SoundManager
    {
        private AudioSource[] _audioSources = new AudioSource[(int)Sound.Max];
        private Dictionary<string, AudioClip> _audioClips = new();
        
        private GameObject _soundRoot = null;

        public void Init()
        {
            if (_soundRoot == null)
            {
                _soundRoot = GameObject.Find("@SoundRoot");
                if (_soundRoot == null)
                {
                    _soundRoot = new GameObject("@SoundRoot");
                    Object.DontDestroyOnLoad(_soundRoot);

                    string[] soundTypeNames = System.Enum.GetNames(typeof(Sound));
                    for (int count = 0; count < soundTypeNames.Length; count++)
                    {
                        GameObject soundObject = new GameObject(soundTypeNames[count]);
                        soundObject.transform.parent = _soundRoot.transform;
                        _audioSources[count] = Utils.GetOrAddComponent<AudioSource>(soundObject);
                    }

                    _audioSources[(int)Sound.BGM].loop = true;
                }
            }
        }

        public void Clear()
        {
            foreach (var audioSource in _audioSources)
            {
                audioSource.Stop();
            }
            
            _audioClips.Clear();
        }
        
        public bool Play(Sound type, string path, float volume = 1.0f, float pitch = 1.0f)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            AudioSource audioSource = _audioSources[(int)type];
            if (path.Contains("Sound/") == false)
            {
                path = $"Sound/{path}";
            }

            audioSource.volume = volume;

            // BGM
            if (type == Sound.BGM)
            {
                AudioClip 
            }
        }

        private AudioClip getAudioClip(string path)
        {
            AudioClip audioClip = null;
            if (_audioClips.TryGetValue(path, out audioClip))
            {
                return audioClip;
            }
            
            audioClip = Managers.
        }
    }
}