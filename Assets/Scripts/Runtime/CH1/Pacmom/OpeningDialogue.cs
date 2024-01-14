using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

namespace Runtime.CH1.Pacmom
{
    public class OpeningDialogue : DialogueViewBase
    {
        private DialogueRunner runner;
        [SerializeField]
        private GameObject speechBubble;
        [SerializeField]
        private GameObject speechBubbleA;
        [SerializeField]
        private GameObject speechBubbleB;
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
            speechBubbleA.SetActive(false);
            speechBubbleB.SetActive(false);
            speechBubble.SetActive(false);

            switch (val)
            {
                case 1: // dust A
                    speechBubbleA.SetActive(true);
                    break;
                case 2: // dust B
                    speechBubbleB.SetActive(true);
                    break;
                case 3: // rapley
                    speechBubble.SetActive(true);
                    break;
            }
        }
    }
}