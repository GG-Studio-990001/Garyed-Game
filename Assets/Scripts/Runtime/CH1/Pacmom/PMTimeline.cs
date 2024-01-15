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
        private GameObject Door;
        [SerializeField]
        private GameObject Sparkle;
        [SerializeField]
        private GameObject[] timeline = new GameObject[2];

        public void OpeningStart_1()
        {
            Door.SetActive(true);
            Sparkle.SetActive(true);
            ControlEnable(false);
        }

        public void OpeningEnd_1()
        {
            Sparkle.SetActive(false);
            ControlEnable(true);
        }

        public void OpeningStart_2()
        {
            ControlEnable(false);
        }

        public void OpeningEnd_2()
        {
            Door.SetActive(false);
            ControlEnable(true);
            Controller.StartGame();

            for (int i=0; i<timeline.Length; i++)
            {
                timeline[i].SetActive(false);
            }
        }

        private void ControlEnable(bool control)
        {
            Rapley.enabled = control;
        }
    }
}