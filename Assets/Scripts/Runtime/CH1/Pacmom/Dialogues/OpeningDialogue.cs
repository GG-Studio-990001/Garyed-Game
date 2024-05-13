using Runtime.ETC;
using System;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Pacmom
{
    public class OpeningDialogue : DialogueViewBase
    {
        private DialogueRunner _runner;

        [SerializeField]
        private PMEnding _ending;
        [SerializeField]
        private RectTransform _speechBubbleA;
        [SerializeField]
        private RectTransform _speechBubbleB;
        [SerializeField]
        private GameObject _rapleyBubble;
        [SerializeField]
        private TextMeshProUGUI _line;
        [SerializeField]
        private GameObject _timeline2;

        private void Awake()
        {
            _runner = GetComponent<DialogueRunner>();
            _runner.AddCommandHandler("DustASpeak", DustASpeak);
            _runner.AddCommandHandler("DustBSpeak", DustBSpeak);
            _runner.AddCommandHandler("RapleySpeak", RapleySpeak);
            _runner.AddCommandHandler("OpeningDialogueFin", OpeningDialogueFin);
        }

        public void StartDialogue()
        {
            if (Managers.Data.IsPacmomCleared)
                _runner.StartDialogue("PMStartAgain");
            else if (!Managers.Data.IsPacmomPlayed)
                _runner.StartDialogue("PMStart");
            else
                _runner.StartDialogue("PMRetry");
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            Managers.Sound.StopSFX();

            string speaker = dialogueLine.CharacterName;

            if (speaker == GlobalConst.DustAStr)
                ResizeSpeechBubble(Speaker.dustA);
            else if (speaker == GlobalConst.DustBStr)
                ResizeSpeechBubble(Speaker.dustB);

            onDialogueLineFinished();
        }

        private void ResizeSpeechBubble(Speaker speaker)
        {
            float height = (_line.text.Length <= 12 ? 200f : 225f);

            if (speaker == Speaker.dustA)
                _speechBubbleA.sizeDelta = new Vector2(_speechBubbleA.sizeDelta.x, height - 12.26f);
            else if (speaker == Speaker.dustB)
                _speechBubbleB.sizeDelta = new Vector2(_speechBubbleB.sizeDelta.x, height);
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
            Managers.Sound.StopSFX();
            ShowSpeechBubble(Speaker.none);
            _timeline2.SetActive(true);
        }

        private void SetLinePos(Speaker speaker)
        {
            float xPos = (speaker == Speaker.dustA ? -474 : 474);

            _line.transform.localPosition = new Vector3(xPos, _line.transform.localPosition.y, _line.transform.localPosition.z);
        }

        private void ShowSpeechBubble(Speaker speaker)
        {
            _speechBubbleA.gameObject.SetActive(false);
            _speechBubbleB.gameObject.SetActive(false);
            _rapleyBubble.SetActive(false);

            switch (speaker)
            {
                case Speaker.dustA:
                    _speechBubbleA.gameObject.SetActive(true);
                    break;
                case Speaker.dustB:
                    _speechBubbleB.gameObject.SetActive(true);
                    break;
                case Speaker.rapley:
                    _rapleyBubble.SetActive(true);
                    break;
            }
        }
    }
}