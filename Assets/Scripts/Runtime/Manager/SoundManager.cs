using Runtime.ETC;
using Runtime.Event;
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
        private AudioClip _currentBGM;
        private float _bgmPlayTime = 0f;

        public AudioSource BGM => _audioSources[(int)Sound.BGM];
        public AudioSource SFX => _audioSources[(int)Sound.SFX];
        public AudioSource Speech => _audioSources[(int)Sound.Speech];
        public AudioSource LuckyBGM => _audioSources[(int)Sound.LuckyBGM];

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
                    for (int count = 0; count < soundTypeNames.Length - 1; count++)
                    {
                        GameObject soundObject = new(soundTypeNames[count]);
                        soundObject.transform.parent = _soundRoot.transform;
                        _audioSources[count] = Utils.GetOrAddComponent<AudioSource>(soundObject);
                    }

                    _audioSources[(int)Sound.BGM].loop = true;

                    SettingsEvent.OnSettingsToggled += OnSettingsToggled;
                }
            }
        }

        private void OnSettingsToggled(bool isOpen)
        {
            if (isOpen)
            {
                SFX.Pause();
            }
            else
            {
                SFX.UnPause();
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

        public bool Play(Sound type, string path, bool isContinue = false) // , float volume = 1.0f, float pitch = 1.0f
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

            // 임시 처리인데 볼륨 잘 바뀜..
            if (type == Sound.BGM || type == Sound.LuckyBGM)
                audioSource.volume = Managers.Data.BgmVolume;
            else
                audioSource.volume = Managers.Data.SfxVolume;

            AudioClip audioClip = GetAudioClip(path);
            
            if (audioClip == null)
            {
                return false;
            }

            // 이미 틀고 있는 브금 또 틀기 방지
            if (type == Sound.BGM || type == Sound.LuckyBGM)
            {
                if (_currentBGM == audioClip)
                {
                    Debug.Log("IsPlaying");
                    return false;
                }
                else
                {
                    _currentBGM = audioClip;
                }
            }
            
            // audioSource.pitch = pitch;
            
            if (type == Sound.SFX)
            {
                audioSource.PlayOneShot(audioClip);
                return true;
            }
            else if (type == Sound.Speech)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                audioSource.clip = audioClip;
                audioSource.Play();
                return true;
            }
            else
            {
                if (BGM.isPlaying)
                {
                    if (audioSource.clip.name.Equals("[Ch1] Main_BGM") || audioSource.clip.name.Equals("[Ch1] Main(Cave)_BGM"))
                        _bgmPlayTime = BGM.time;
                }
                StopBGM();
                
                audioSource.clip = audioClip;

                Debug.Log("_bgmPlayTime " + _bgmPlayTime);
                Debug.Log("isContinue " + isContinue);
                // 브금 특정 시간부터 이어서 틀기
                if (isContinue && type == Sound.BGM)
                {
                    audioSource.time = _bgmPlayTime;
                }
                else
                {
                    audioSource.time = 0;
                }

                audioSource.Play();
                return true;
            }

            // return false;
        }

        public void PlayRandomSpeech(string folder)
        {
            string path = $"Sound/{folder}";
            AudioClip audioClip = Managers.Resource.LoadRandom<AudioClip>(path);

            path += $"/{audioClip.name}";

            Play(Sound.Speech, path);
        }

        private AudioClip GetAudioClip(string path)
        {
            if (_audioClips.TryGetValue(path, out AudioClip audioClip))
            {
                return audioClip;
            }

            audioClip = Managers.Resource.Load<AudioClip>(path);
            _audioClips.Add(path, audioClip);
            return audioClip;
        }

        public void StopAllSound()
        {
            StopBGM();
            StopSFX();
        }

        public void StopBGM()
        {
            BGM.Stop();
            LuckyBGM.Stop();
        }

        public void StopSFX()
        {
            SFX.Stop();
        }
    }
}