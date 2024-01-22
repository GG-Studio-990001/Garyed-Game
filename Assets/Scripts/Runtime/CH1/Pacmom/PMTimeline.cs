using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.CH1.Pacmom
{
    public class PMTimeline : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput playerInput;
        [SerializeField]
        private PMGameController Controller;
        [SerializeField]
        private GameObject OpeningUI;
        [SerializeField]
        private GameObject[] dialogueRunner = new GameObject[2];
        [SerializeField]
        private GameObject[] timeline = new GameObject[2];
        [SerializeField]
        private SpriteRenderer[] characters = new SpriteRenderer[2];

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

        public void FlipCharacters()
        {
            for (int i = 0; i < characters.Length; i++)
                characters[i].flipX = false;
        }

        public void PacmomSizeUp()
        {
            characters[0].transform.localScale = new Vector3(80f, 80f, 80f);
        }

        public void ControlEnable(bool control)
        {
            playerInput.enabled = control;
        }
    }
}