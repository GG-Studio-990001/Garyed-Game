using TMPro;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Pacmom
{
    public class OpeningDialogue : DialogueViewBase
    {
        private DialogueRunner runner;
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
            // runner.AddCommandHandler("RapleyQMark", RapleyQMark);
            runner.AddCommandHandler("OpeningDialogueFin", OpeningDialogueFin);
        }

        public void DustASpeak()
        {
            speechBubbleA.SetActive(true);
            speechBubbleB.SetActive(false);

            line.transform.localPosition = new Vector3(-474f, line.transform.localPosition.y, line.transform.localPosition.z);
        }

        public void DustBSpeak()
        {
            speechBubbleA.SetActive(false);
            speechBubbleB.SetActive(true);

            line.transform.localPosition = new Vector3(474f, line.transform.localPosition.y, line.transform.localPosition.z);
        }

        public void RapleyQMark()
        {
            Debug.Log("?");
        }

        public void OpeningDialogueFin()
        {
            speechBubbleA.SetActive(false);
            speechBubbleB.SetActive(false);

            timeline_2.SetActive(true);
        }
    }
}