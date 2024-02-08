using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class DustTalkSound : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] effectSounds;
        [SerializeField]
        private AudioSource effectAudioSource;

        public void DustTalkingSFX()
        {
            int rand = Random.Range(0, effectSounds.Length);
            AudioClip audio = effectSounds[rand];

            effectAudioSource.PlayOneShot(audio);
        }
    }
}