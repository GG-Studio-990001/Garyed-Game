using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class DustTalkSound : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] _effectSounds;
        [SerializeField]
        private AudioSource _effectAudioSource;

        public void DustTalkingSFX()
        {
            int rand = Random.Range(0, _effectSounds.Length);
            AudioClip audio = _effectSounds[rand];

            _effectAudioSource.PlayOneShot(audio);
        }
    }
}