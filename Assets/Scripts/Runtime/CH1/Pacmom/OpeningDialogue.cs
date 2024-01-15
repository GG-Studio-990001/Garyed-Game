using System;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Pacmom
{
    public class OpeningDialogue : DialogueViewBase
    {
        private DialogueRunner runner;
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

        void Awake()
        {
            runner = GetComponent<DialogueRunner>();
            runner.AddCommandHandler("DustASpeak", DustASpeak);
            runner.AddCommandHandler("DustBSpeak", DustBSpeak);
            runner.AddCommandHandler("RapleySpeak", RapleySpeak);
            runner.AddCommandHandler("OpeningDialogueFin", OpeningDialogueFin);
        }

        public void VacuumDialogue()
        {
            runner.StartDialogue("PMVacuumMode");
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            string speaker = dialogueLine.CharacterName;

            if (speaker == "먼지유령1")
                ResizeSpeechBubble(1);
            else if (speaker == "먼지유령2")
                ResizeSpeechBubble(2);

            onDialogueLineFinished();
        }

        private void ResizeSpeechBubble(int num)
        {
            RectTransform bubble = (num == 1 ? speechBubbleA : speechBubbleB);

            string text = line.text;

            if (text.Length <= 12)
            {
                bubble.sizeDelta = new Vector2(speechBubbleA.sizeDelta.x, 200f);
            }
            else if (text.Length <= 30)
            {
                bubble.sizeDelta = new Vector2(speechBubbleA.sizeDelta.x, 250f);
            }
            else
            {
                bubble.sizeDelta = new Vector2(speechBubbleA.sizeDelta.x, 300f);
            }

            if (num == 1)
                speechBubbleA.sizeDelta = bubble.sizeDelta;
            else
                speechBubbleB.sizeDelta = bubble.sizeDelta;
        }

        public void DustASpeak()
        {
            ShowSpeechBubble(1);
            SetXPos(-474f);
        }

        public void DustBSpeak()
        {
            ShowSpeechBubble(2);
            SetXPos(474f);
        }

        public void RapleySpeak()
        {
            ShowSpeechBubble(3);
            SetXPos(0);
        }

        public void OpeningDialogueFin()
        {
            ShowSpeechBubble();
            timeline_2.SetActive(true);
        }

        private void SetXPos(float xPos)
        {
            line.transform.localPosition = new Vector3(xPos, line.transform.localPosition.y, line.transform.localPosition.z);

            if (xPos == 0)
                SetFontSize(50);
            else
                SetFontSize(33);
        }

        private void SetFontSize(int size)
        {
            line.fontSize = size;
        }

        private void ShowSpeechBubble(int val = 0)
        {
            speechBubbleA.gameObject.SetActive(false);
            speechBubbleB.gameObject.SetActive(false);
            speechBubble.SetActive(false);

            switch (val)
            {
                case 1: // dust A
                    speechBubbleA.gameObject.SetActive(true);
                    break;
                case 2: // dust B
                    speechBubbleB.gameObject.SetActive(true);
                    break;
                case 3: // rapley
                    speechBubble.SetActive(true);
                    break;
            }
        }
    }
}