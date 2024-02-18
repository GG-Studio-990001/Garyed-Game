using Runtime.InGameSystem;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Runtime.CH1.Main.Dialogue
{
    public class TimelineController : MonoBehaviour
    {
        [SerializeField] private TimelineAsset[] _timelineAssets;
        [field:SerializeField] public PlayableDirector PlayableDirector { get; private set; }

        private void Awake()
        {
 
        }
        
        public void PlayTimeline(int minor)
        {
            if (_timelineAssets.Length <= minor)
            {
                Debug.LogError($"TimelineAsset with minor {minor} not found");
                return;
            }
            PlayableDirector.playableAsset = _timelineAssets[minor];
            PlayableDirector.Play();
        }
    }
}
