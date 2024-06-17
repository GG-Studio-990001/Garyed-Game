using Runtime.ETC;
using System;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

namespace Runtime.CH2.Main
{
    public enum Character
    {
        rapley = 0,
        r2mon = 1,
    }

    public class CH2Dialogue : DialogueViewBase
    {
        private DialogueRunner _runner;
        [SerializeField]
        private Image[] _characters = new Image[2];
        [SerializeField]
        private GameObject next;
        private string _speaker;

        private void Awake()
        {
            _runner = GetComponent<DialogueRunner>();
            _runner.AddCommandHandler("DialogueFin", HighlightAll);
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            _speaker = dialogueLine.CharacterName;

            if (_speaker.Equals("라플리"))
                StandingHighlight((int)Character.rapley);
            else if (_speaker.Equals("R2-Mon"))
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

        public void TextSFX()
        {
            if (_speaker == "R2-Mon")
            {
                Managers.Sound.Play(Sound.Speech, "[CH1] R2-Mon_Text_SFX_04");
            }
            else
            {
                Managers.Sound.Play(Sound.Speech, "[CH1] Text SFX");
            }
        }
    }
}