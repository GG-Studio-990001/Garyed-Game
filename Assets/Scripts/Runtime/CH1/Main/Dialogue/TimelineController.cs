using Runtime.InGameSystem;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Runtime.CH1.Main.Dialogue
{
    public class TimelineController : MonoBehaviour
    {
        [SerializeField] private TimelineAsset[] _timelineAssets;
        private PlayableDirector _playableDirector;

        private void Awake()
        {
            _playableDirector = GetComponent<PlayableDirector>();
            
            _playableDirector.played += director => DataProviderManager.Instance.ControlsDataProvider.Get().RestrictPlayerInput();
            _playableDirector.stopped += director => DataProviderManager.Instance.ControlsDataProvider.Get().ReleasePlayerInput();
        }
        
        public void PlayTimeline(int minor)
        {
            _playableDirector.playableAsset = _timelineAssets[minor];
            _playableDirector.Play();
        }
    }
}
