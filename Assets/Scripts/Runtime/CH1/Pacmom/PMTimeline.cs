using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.CH1.Pacmom
{
    public class PMTimeline : MonoBehaviour
    {
        [SerializeField]
        private PlayerInput _playerInput;
        [SerializeField]
        private PMGameController _controller;
        [SerializeField]
        private GameObject _openingUI;
        [SerializeField]
        private GameObject[] _dialogueRunner = new GameObject[2];
        [SerializeField]
        private GameObject[] _timeline = new GameObject[2];
        [SerializeField]
        private SpriteRenderer _pacmom;
        [SerializeField]
        private SpriteRenderer _rapley;

        public void OpeningFinish()
        {
            _openingUI.SetActive(false);

            for (int i=0; i< _timeline.Length; i++)
                _timeline[i].SetActive(false);

            _dialogueRunner[0].SetActive(false);
            _dialogueRunner[1].SetActive(true);

            ControlEnable(true);
            _controller.StartGame();
        }

        public void FlipCharacters()
        {
            _pacmom.flipX = false;
            _rapley.flipX = false;
        }

        public void PacmomSizeUp()
        {
            _pacmom.transform.localScale = new Vector3(80f, 80f, 80f);
        }

        public void ControlEnable(bool control)
        {
            _playerInput.enabled = control;
        }
    }
}