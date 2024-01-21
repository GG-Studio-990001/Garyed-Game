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
            if (bubbleA.activeInHierarchy)
                SetBubble(dustAObj, bubbleA, textA);

            if (bubbleB.activeInHierarchy)
                SetBubble(dustBObj, bubbleB, textB);
        }

        private void SetBubble(GameObject dust, GameObject bubble, TextMeshProUGUI text)
        {
            float xPos = (dust.transform.position.x > 0 ? -2.3f : 2.3f);
            float yRotate = (dust.transform.position.x > 0 ? 0f : -180f);

            bubble.transform.position = new Vector3(dust.transform.position.x + xPos,
            dust.transform.position.y + 1.3f, dust.transform.position.z);

            bubble.transform.rotation = Quaternion.Euler(0f, yRotate, 0f);
            text.transform.rotation = Quaternion.Euler(0f, yRotate * 2f, 0f); // 왜 이렇게해야되지..
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
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