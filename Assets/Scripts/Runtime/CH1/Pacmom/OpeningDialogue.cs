using Runtime.ETC;
using System;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Pacmom
{
    public class OpeningDialogue : DialogueViewBase
    {
        private DialogueRunner runner;
        private readonly int dustA = 0, dustB = 1, rapley = 2;

        [SerializeField]
        private GameObject speechBubble;
        [SerializeField]
        private RectTransform speechBubbleA;
        [SerializeField]
        private RectTransform speechBubbleB;
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

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            string speaker = dialogueLine.CharacterName;

            if (speaker == GlobalConst.DustAStr)
                ResizeSpeechBubble(dustA);
            else
                ResizeSpeechBubble(dustB);

            onDialogueLineFinished();
        }

        private void ResizeSpeechBubble(int speaker)
        {
            RectTransform bubble = (speaker == 0 ? speechBubbleA : speechBubbleB);

            string text = line.text;

            if (text.Length <= 12)
            {
                bubble.sizeDelta = new Vector2(bubble.sizeDelta.x, 200f);
            }
            else if (text.Length <= 29)
            {
                bubble.sizeDelta = new Vector2(bubble.sizeDelta.x, 250f);
            }
            else
            {
                bubble.sizeDelta = new Vector2(bubble.sizeDelta.x, 300f);
            }

            if (speaker == 0)
                speechBubbleA.sizeDelta = bubble.sizeDelta;
            else
                speechBubbleB.sizeDelta = bubble.sizeDelta;
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
            SetLinePos(rapley);
        }

        public void OpeningDialogueFin()
        {
            ShowSpeechBubble();
            timeline_2.SetActive(true);
        }

        private void SetLinePos(int speaker)
        {
            float xPos = -474, yPos = 201;
            float fontSize = 33;

            switch (speaker)
            {
                case 0: // dust A
                    break;
                case 1: // dust B
                    xPos *= -1;
                    break;
                case 2: // rapley
                    xPos = 0;
                    yPos = 185;
                    fontSize = 75;
                    break;
            }

            line.transform.localPosition = new Vector3(xPos, yPos, line.transform.localPosition.z);
            line.fontSize = fontSize;
        }

        private void ShowSpeechBubble(int speaker = -1)
        {
            speechBubbleA.gameObject.SetActive(false);
            speechBubbleB.gameObject.SetActive(false);
            speechBubble.SetActive(false);

            switch (speaker)
            {
                case 0: // dust A
                    speechBubbleA.gameObject.SetActive(true);
                    break;
                case 1: // dust B
                    speechBubbleB.gameObject.SetActive(true);
                    break;
                case 2: // rapley
                    speechBubble.SetActive(true);
                    break;
            }
        }
    }
}