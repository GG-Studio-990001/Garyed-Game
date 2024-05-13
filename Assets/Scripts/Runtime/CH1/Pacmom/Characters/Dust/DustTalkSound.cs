using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class DustTalkSound : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] _sfxSounds;
        [SerializeField]
        private AudioSource _sfxAudioSource;

        public void DustTalkingSFX()
        {
            int rand = Random.Range(0, _sfxSounds.Length);
            AudioClip audio = _sfxSounds[rand];

            _sfxAudioSource.volume = Managers.Data.SfxVolume;
            _sfxAudioSource.PlayOneShot(audio);
        }
    }
}