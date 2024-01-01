using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;

public class TimelineTest : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;
    
    private void Start()
    {
        PlayableAsset playableAsset = Addressables.LoadAssetAsync<PlayableAsset>("Assets/Timeline/CutScene.playable").WaitForCompletion();
        _playableDirector.Play(playableAsset);
    }
}
