using Runtime.ETC;
using Runtime.Interface.Pacmom;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Pacmom : MonoBehaviour, ICharacter
    {
        public PMGameController gameController;
        public MovementAndRotation movement { get; set; }
        public AI ai { get; set; }
        [SerializeField]
        private GameObject vacuum;

        private void Awake()
        {
            movement = GetComponent<MovementAndRotation>();
            ai = GetComponent<AI>();
        }

        private void Start()
        {
            SetSpriteRotation();
            SetAIValue();
            ResetState();
        }

        private void SetAIValue()
        {
            ai?.SetStronger(false);
            ai?.SetCoinMatter(true);
        }

        private void SetSpriteRotation()
        {
            movement.spriteRotation.SetCanRotate(true);
            movement.spriteRotation.SetCanFlip(true);
        }

        public void ResetState()
        {
            SetRotateToZero();
            movement.ResetState();
        }

        private void FixedUpdate()
        {
            movement.Move();
        }

        public void VacuumMode(bool isVacuum)
        {
            SetRotateToZero();

            ai.SetStronger(isVacuum);
            movement.spriteRotation.SetCanRotate(!isVacuum);
            vacuum.SetActive(isVacuum);
        }

        public void SetRotateToZero()
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                if (ai.isStronger)
                    gameController?.RapleyEaten();
                else
                    gameController?.PacmomEatenByRapley();
            }
        }
    }
}
