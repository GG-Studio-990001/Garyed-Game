using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Pacmom : MonoBehaviour
    {
        public PMGameController gameController;
        public MovementAndRotation movement { get; private set; }
        public AI ai { get; private set; }
        [SerializeField]
        private GameObject vacuum;

        private void Awake()
        {
            // 테스트코드를 위해 이렇게 했는데 뭔가 별로임..
            if (GetComponent<MovementAndRotation>() != null)
            {
                movement = GetComponent<MovementAndRotation>();
            }

            if (GetComponent<AI>() != null)
            {
                ai = GetComponent<AI>();
            }
        }

        private void Start()
        {
            SetSpriteRotation();
            SetAIValue();
            ResetState();
        }

        public void SetMovement(MovementAndRotation movement)
        {
            this.movement = movement;
        }

        public void SetAI(AI ai)
        {
            this.ai = ai;
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
                    gameController?.PacmomEaten(GlobalConst.PlayerStr);
            }
        }
    }
}
