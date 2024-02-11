using Runtime.ETC;
using System;
using TMPro;
using UnityEngine;
using Yarn.Unity;
using Runtime.InGameSystem;
using Yarn;

namespace Runtime.CH1.Pacmom
{
    public class OpeningDialogue : DialogueViewBase
    {
        private DialogueRunner runner;
        private readonly int dustA = 0, dustB = 1, rapley = 2;

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

            if (speaker == GlobalConst.DustAStr || speaker == GlobalConst.DustBStr)
            {
                if (speaker == GlobalConst.DustAStr)
                    ResizeSpeechBubble(dustA);
                else
                    ResizeSpeechBubble(dustB);
            }

            onDialogueLineFinished();
        }

        private void ResizeSpeechBubble(int speaker)
        {
            float height = (line.text.Length <= 12 ? 200f : 225f);

            if (speaker == 0)
                speechBubbleA.sizeDelta = new Vector2(speechBubbleA.sizeDelta.x, height - 12.26f);
            else
                speechBubbleB.sizeDelta = new Vector2(speechBubbleB.sizeDelta.x, height);
        }

        public void DustASpeak()
        {
            ShowSpeechBubble(dustA);
            SetLinePos(dustA);
        }

        public void DustBSpeak()
        {
            ShowSpeechBubble(dustB);
            SetLinePos(dustB);
        }

        public void RapleySpeak()
        {
            ShowSpeechBubble(rapley);
        }

        public void OpeningDialogueFin()
        {
            soundSystem.StopSFX();
            ShowSpeechBubble();
            timeline_2.SetActive(true);
        }

        private void SetLinePos(int speaker)
        {
            float xPos = (speaker == 0 ? -474 : 474);

            line.transform.localPosition = new Vector3(xPos, line.transform.localPosition.y, line.transform.localPosition.z);
        }

        private void ShowSpeechBubble(int speaker = -1)
        {
            speechBubbleA.gameObject.SetActive(false);
            speechBubbleB.gameObject.SetActive(false);
            rapleyBubble.SetActive(false);

            switch (speaker)
            {
                case 0: // dust A
                    speechBubbleA.gameObject.SetActive(true);
                    break;
                case 1: // dust B
                    speechBubbleB.gameObject.SetActive(true);
                    break;
                case 2: // rapley
                    rapleyBubble.SetActive(true);
                    break;
            }
        }
    }
}