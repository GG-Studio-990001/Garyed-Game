using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineController : MonoBehaviour
{
    private BoxCollider2D _boxCollider2D;
    private PlayableDirector _playableDirector;
    private SignalReceiver _signalReceiver;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _playableDirector = GetComponent<PlayableDirector>();
        _signalReceiver = GetComponent<SignalReceiver>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _boxCollider2D.enabled = false;
            _playableDirector.Play();
        }
    }
}
