using UnityEngine;
using UnityEngine.InputSystem;
using Yarn;
using Yarn.Unity;

namespace Runtime.CH1.Pacmom
{
    public class PMTimeline : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput Rapley;
        [SerializeField]
        private PMGameController Controller;
        [SerializeField]
        private GameObject OpeningUI;
        [SerializeField]
        private GameObject[] dialogueRunner = new GameObject[2];
        [SerializeField]
        private GameObject[] timeline = new GameObject[2];

        public void OpeningFinish()
        {
            OpeningUI.SetActive(false);

            for (int i=0; i<timeline.Length; i++)
                timeline[i].SetActive(false);

            dialogueRunner[0].SetActive(false);
            dialogueRunner[1].SetActive(true);

            ControlEnable(true);
            Controller.StartGame();
        }

        public void ControlEnable(bool control)
        {
            Rapley.enabled = control;
        }
    }
}