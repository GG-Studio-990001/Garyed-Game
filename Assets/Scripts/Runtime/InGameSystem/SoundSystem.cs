using Runtime.Data;
using UnityEngine;

namespace Runtime.InGameSystem
{
    public class SoundSystem : MonoBehaviour
    {
        [SerializeField] private Sound[] backgroundSounds;
        [SerializeField] private Sound[] effectSounds;
        [SerializeField] private AudioSource backgroundAudioSource;
        [SerializeField] private AudioSource effectAudioSource;
    
        public void PlayMusic(string soundName)
        {
            Sound sound = FindSound(backgroundSounds, soundName);
            if (sound == null)
            {
                Debug.LogWarning($"Cannot find sound {soundName}");
                return;
            }
        
            backgroundAudioSource.clip = sound.clip;
            backgroundAudioSource.Play();
        }

        public void StopMusic()
        {
            backgroundAudioSource.Stop();
        }
    
        public void PlayEffect(string soundName)
        {
            Sound sound = FindSound(effectSounds, soundName);
            if (sound == null)
            {
                Debug.LogWarning($"Cannot find sound {soundName}");
                return;
            }
        
            effectAudioSource.PlayOneShot(sound.clip);
        }
    
        private Sound FindSound(Sound[] sounds, string soundName) => System.Array.Find(sounds, sound => sound.name == soundName);
    }
}
