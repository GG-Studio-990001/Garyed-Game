using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Dust : MonoBehaviour
    {
        public PMGameController gameController;
        public MovementAndEyes movement { get; private set; }
        public AI ai { get; private set; }
        [field:SerializeField]
        public int dustID { get; private set; }

        private void Awake()
        {
            if (GetComponent<MovementAndEyes>() != null)
            {
                movement = GetComponent<MovementAndEyes>();
            }

            if (GetComponent<AI>() != null)
            {
                ai = GetComponent<AI>();
            }
        }

        private void Start()
        {
            SetAIValue();
            ResetState();
        }

        public void SetMovement(MovementAndEyes movement)
        {
            this.movement = movement;
        }

        public void SetAI(AI ai)
        {
            this.ai = ai;
        }

        private void SetAIValue()
        {
            ai?.SetStronger(true);
            ai?.SetCoinMatter(false);
        }

        public void ResetState()
        {
            movement.SetEyeNormal(true);
            movement.GetEyeSpriteByPosition();
            movement.ResetState();
        }

        private void FixedUpdate()
        {
            movement.Move();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PacmomStr))
            {
                if (ai.isStronger)
                    gameController?.PacmomEaten(GlobalConst.DustStr, dustID);
                else
                    gameController?.DustEaten(this);
            }
        }
    }
}