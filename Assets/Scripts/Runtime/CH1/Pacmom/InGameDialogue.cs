using System;
using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Pacmom
{
    public class InGameDialogue : DialogueViewBase
    {
        private DialogueRunner runner;

        [SerializeField]
        private GameObject dustAObj;
        [SerializeField]
        private GameObject bubbleA;
        [SerializeField]
        private TextMeshProUGUI textA;
        [SerializeField]
        private GameObject dustBObj;
        [SerializeField]
        private GameObject bubbleB;
        [SerializeField]
        private TextMeshProUGUI textB;

        private void Awake()
        {
            runner = GetComponent<DialogueRunner>();
            runner.AddCommandHandler("WarningStart", WarningStart);
            runner.AddCommandHandler("WarningEnd", WarningEnd);
        }

        private void Update()
        {
            // TODO: 말풍선이 화면 밖으로 나가지 않도록 위치 조정

            if (bubbleA.activeInHierarchy)
            {
                bubbleA.transform.position = new Vector3(dustAObj.transform.position.x - 2.3f,
                dustAObj.transform.position.y + 1.3f, dustAObj.transform.position.z);
            }

            if (bubbleB.activeInHierarchy)
            {
                bubbleB.transform.position = new Vector3(dustBObj.transform.position.x + 2.3f,
                dustBObj.transform.position.y + 1.3f, dustBObj.transform.position.z);
            }
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            Debug.Log("WarningStart");

            string speaker = dialogueLine.CharacterName;
            TextMeshProUGUI text = (speaker == "먼지유령1" ? textA : textB);

            text.text = dialogueLine.TextWithoutCharacterName.Text;

            if (speaker == "먼지유령1")
                textA.text = text.text;
            else
                textB.text = text.text;

            onDialogueLineFinished();
        }

        public void RandomDialogue()
        {
            runner.Stop();
            runner.StartDialogue("PMRandom");
        }

        #region Vaccum
        public void VacuumDialogue()
        {
            runner.Stop();
            runner.StartDialogue("PMVacuumMode");
        }
        
        public void WarningStart()
        {
            Debug.Log("WarningStart");
            bubbleA.SetActive(true);
            bubbleB.SetActive(true);
        }

        public void WarningEnd()
        {
            bubbleA.SetActive(false);
            bubbleB.SetActive(false);
        }
        #endregion
    }
}