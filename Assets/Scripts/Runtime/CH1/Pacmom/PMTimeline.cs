using UnityEngine;
using UnityEngine.InputSystem;

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

        public void OpeningStart()
        {
            UI.SetActive(false);
            Door.SetActive(true);
            ControlEnable(false);
        }

        public void OpeningFinish()
        {
            UI.SetActive(true);
            Door.SetActive(false);
            ControlEnable(true);
            Controller.StartGame();
        }

        private void ControlEnable(bool control)
        {
            Rapley.enabled = control;
        }
    }
}