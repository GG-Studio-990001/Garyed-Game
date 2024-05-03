using System;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Runtime.CH2
{
    public enum Character
    {
        rapley = 0,
        r2mon = 1,
    }

    public class IntroDialogue : DialogueViewBase
    {
        private DialogueRunner _runner;
        [SerializeField]
        private Image[] _characters = new Image[2];
        [SerializeField]
        private GameObject next;

        private void Awake()
        {
            _runner = GetComponent<DialogueRunner>();
            _runner.AddCommandHandler("DialogueFin", HighlightAll);
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            string speaker = dialogueLine.CharacterName;
            Debug.Log(speaker);

            if (speaker.Equals("라플리"))
                StandingHighlight((int)Character.rapley);
            else if (speaker.Equals("R2-MON"))
                StandingHighlight((int)Character.r2mon);

            onDialogueLineFinished();
        }

        private void HighlightAll()
        {
            for (int i = 0; i < _characters.Length; i++)
                _characters[i].gameObject.SetActive(false);
            next.SetActive(true);
        }

        private void StandingHighlight(int num)
        {
            Color speaker = new Color32(255, 255, 255, 255);
            Color nonSpeaker = new Color32(157, 157, 157, 255);

            _characters[num].color = speaker;
            _characters[1 - num].color = nonSpeaker;
        }
    }
}