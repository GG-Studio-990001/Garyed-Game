using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.CH1.Pacmom
{
    public class PMTimeline : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;
        [SerializeField]
        private PMGameController controller;
        [SerializeField]
        private GameObject openingUI;
        [SerializeField]
        private GameObject[] dialogueRunner = new GameObject[2];
        [SerializeField]
        private GameObject[] timeline = new GameObject[2];
        [SerializeField]
        private SpriteRenderer pacmom;
        [SerializeField]
        private SpriteRenderer rapley;

        public void OpeningFinish()
        {
            openingUI.SetActive(false);

            for (int i=0; i<timeline.Length; i++)
                timeline[i].SetActive(false);

            dialogueRunner[0].SetActive(false);
            dialogueRunner[1].SetActive(true);

            ControlEnable(true);
            controller.StartGame();
        }

        public void FlipCharacters()
        {
            pacmom.flipX = false;
            rapley.flipX = false;
        }

        public void PacmomSizeUp()
        {
            pacmom.transform.localScale = new Vector3(80f, 80f, 80f);
        }

        public void ControlEnable(bool control)
        {
            playerInput.enabled = control;
        }
    }
}