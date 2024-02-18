using Runtime.ETC;
using System;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using Runtime.InGameSystem;

namespace Runtime.CH1.Pacmom
{
    public class OpeningDialogue : DialogueViewBase
    {
        private DialogueRunner runner;

        [SerializeField]
        private SoundSystem soundSystem;
        [SerializeField]
        private PMEnding ending;
        [SerializeField]
        private RectTransform speechBubbleA;
        [SerializeField]
        private RectTransform speechBubbleB;
        [SerializeField]
        private GameObject rapleyBubble;
        [SerializeField]
        private TextMeshProUGUI line;
        [SerializeField]
        private GameObject timeline_2;
        private readonly string dustAStr = "dustA";
        private readonly string dustBStr = "dustB";

        private void Awake()
        {
            runner = GetComponent<DialogueRunner>();
            runner.AddCommandHandler("DustASpeak", DustASpeak);
            runner.AddCommandHandler("DustBSpeak", DustBSpeak);
            runner.AddCommandHandler("RapleySpeak", RapleySpeak);
            runner.AddCommandHandler("OpeningDialogueFin", OpeningDialogueFin);
        }

        public void StartDialogue()
        {
            if (!ending.isGameClear)
                runner.StartDialogue("PMStart");
            else
                runner.StartDialogue("PMRetry");
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            soundSystem.StopSFX();

            string speaker = dialogueLine.CharacterName;

            if (speaker == dustAStr)
                ResizeSpeechBubble(Speaker.dustA);
            else if (speaker == dustBStr)
                ResizeSpeechBubble(Speaker.dustB);

            onDialogueLineFinished();
        }

        private void ResizeSpeechBubble(Speaker speaker)
        {
            float height = (line.text.Length <= 12 ? 200f : 225f);

            if (speaker == Speaker.dustA)
                speechBubbleA.sizeDelta = new Vector2(speechBubbleA.sizeDelta.x, height - 12.26f);
            else if (speaker == Speaker.dustB)
                speechBubbleB.sizeDelta = new Vector2(speechBubbleB.sizeDelta.x, height);
        }

        public void DustASpeak()
        {
            ShowSpeechBubble(Speaker.dustA);
            SetLinePos(Speaker.dustA);
        }

        public void DustBSpeak()
        {
            ShowSpeechBubble(Speaker.dustB);
            SetLinePos(Speaker.dustB);
        }

        public void RapleySpeak()
        {
            ShowSpeechBubble(Speaker.rapley);
        }

        public void OpeningDialogueFin()
        {
            soundSystem.StopSFX();
            ShowSpeechBubble(Speaker.none);
            timeline_2.SetActive(true);
        }

        private void SetLinePos(Speaker speaker)
        {
            float xPos = (speaker == Speaker.dustA ? -474 : 474);

            line.transform.localPosition = new Vector3(xPos, line.transform.localPosition.y, line.transform.localPosition.z);
        }

        private void ShowSpeechBubble(Speaker speaker)
        {
            speechBubbleA.gameObject.SetActive(false);
            speechBubbleB.gameObject.SetActive(false);
            rapleyBubble.SetActive(false);

            switch (speaker)
            {
                case Speaker.dustA:
                    speechBubbleA.gameObject.SetActive(true);
                    break;
                case Speaker.dustB:
                    speechBubbleB.gameObject.SetActive(true);
                    break;
                case Speaker.rapley:
                    rapleyBubble.SetActive(true);
                    break;
            }
        }
    }
}