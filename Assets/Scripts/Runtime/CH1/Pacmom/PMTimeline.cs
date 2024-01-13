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
        private GameObject UI;
        [SerializeField]
        private GameObject Door;
        [SerializeField]
        private GameObject Sparkle;
        [SerializeField]
        private GameObject[] dialouges;

        public void OpeningStart_1()
        {
            UI.SetActive(false);
            Door.SetActive(true);
            Sparkle.SetActive(true);
            ControlEnable(false);
        }

        public void OpeningEnd_1()
        {
            Sparkle.SetActive(false);
            ControlEnable(true);
            foreach (var d in dialouges)
            {
                d.SetActive(true);
            }
        }

        /*
        public void OpeningStart_2()
        {
            ControlEnable(false);
        }

        public void OpeningEnd_2()
        {
            UI.SetActive(true);
            Door.SetActive(false);
            ControlEnable(true);
            Controller.StartGame();
        }*/

        private void ControlEnable(bool control)
        {
            Rapley.enabled = control;
        }
    }
}