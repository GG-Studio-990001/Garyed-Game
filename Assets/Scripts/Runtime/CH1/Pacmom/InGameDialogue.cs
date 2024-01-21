using Runtime.ETC;
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
        private GameObject dustA;
        [SerializeField]
        private GameObject bubbleA;
        [SerializeField]
        private TextMeshProUGUI textA;
        [SerializeField]
        private GameObject dustB;
        [SerializeField]
        private GameObject bubbleB;
        [SerializeField]
        private TextMeshProUGUI textB;
        [SerializeField]
        private float currentTime = 0f;
        [SerializeField]
        private float targetTime = 10f;

        private void Awake()
        {
            runner = GetComponent<DialogueRunner>();
            runner.AddCommandHandler("HideBubble", HideBubble);
        }

        private void Update()
        {
            CheckTime();

            if (bubbleA.activeInHierarchy)
                SetBubble(dustA, bubbleA, textA);

            if (bubbleB.activeInHierarchy)
                SetBubble(dustB, bubbleB, textB);
        }

        private void SetBubble(GameObject dust, GameObject bubble, TextMeshProUGUI text)
        {
            float xPos = (dust.transform.position.x > 0 ? -2.3f : 2.3f);
            float yRotate = (dust.transform.position.x > 0 ? 0f : -180f);

            bubble.transform.position = new Vector3(dust.transform.position.x + xPos,
            dust.transform.position.y + 1.3f, dust.transform.position.z);

            bubble.transform.rotation = Quaternion.Euler(0f, yRotate, 0f);
            text.transform.rotation = Quaternion.Euler(0f, yRotate * 2f, 0f); // 왜지..
        }

        public override void RunLine(LocalizedLine dialogueLine, Action onDialogueLineFinished)
        {
            string speaker = dialogueLine.CharacterName;

            // 화자 미정일 때
            if (speaker == GlobalConst.DustStr)
            {
                int rand = UnityEngine.Random.Range(0, 2);
                speaker = (rand == 0 ? GlobalConst.DustAStr : GlobalConst.DustBStr);
            }

            TextMeshProUGUI text = (speaker == GlobalConst.DustAStr ? textA : textB);

            text.text = dialogueLine.TextWithoutCharacterName.Text;

            if (speaker == GlobalConst.DustAStr)
            {
                bubbleA.SetActive(true);
                textA.text = text.text;
            }
            else
            {
                bubbleB.SetActive(true);
                textB.text = text.text;
            }
            
            onDialogueLineFinished();
        }

        public void HideBubble()
        {
            if (bubbleA.activeSelf)
                bubbleA.SetActive(false);
            if (bubbleB.activeSelf)
                bubbleB.SetActive(false);
        }

        #region Random
        private void CheckTime()
        {
            if (dustA.GetComponent<AI>().isStronger)
                currentTime += Time.deltaTime;

            if (targetTime < currentTime)
                ShowRandom();
        }

        private void ShowRandom()
        {
            RandomDialogue();

            currentTime = 0;
            targetTime = UnityEngine.Random.Range(5f, 10f);
        }

        private void RandomDialogue()
        {
            runner.Stop();
            runner.StartDialogue("PMRandom");
        }
        #endregion

        #region Vacuum
        public void VacuumDialogue()
        {
            runner.Stop();
            runner.StartDialogue("PMVacuumMode");
        }
        #endregion
    }
}