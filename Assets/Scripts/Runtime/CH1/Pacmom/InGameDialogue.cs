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
        private GameObject dustBObj;
        [SerializeField]
        private GameObject bubbleB;

        private void Awake()
        {
            runner = GetComponent<DialogueRunner>();
            runner.AddCommandHandler("DustATell", DustATell);
            runner.AddCommandHandler("DustBTell", DustBTell);
            runner.AddCommandHandler("WarningEnd", WarningEnd);
        }

        private void Update()
        {
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
        

        public void DustATell()
        {
            bubbleA.SetActive(true);
        }

        public void DustBTell()
        {
            bubbleA.SetActive(false);
            bubbleB.SetActive(true);
        }

        public void WarningEnd()
        {
            bubbleB.SetActive(false);
        }
        #endregion
    }
}