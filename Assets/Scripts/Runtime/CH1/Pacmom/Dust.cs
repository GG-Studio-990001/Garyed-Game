using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Dust : MonoBehaviour
    {
        [SerializeField]
        private InGameDialogue dialogue;

        public PMGameController gameController;
        public MovementAndEyes movement { get; private set; }
        public AI ai { get; private set; }

        [field:SerializeField]
        public int dustID { get; private set; }
        [SerializeField]
        private float reachTime = 0f;
        private bool dustTalked = false;

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

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                reachTime += Time.deltaTime;
            }

            if (!dustTalked && reachTime > 1.2f)
            {
                if (ai.isStronger)
                    dialogue.BlockedDialogue(dustID);
                dustTalked = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            reachTime = 0f;
            dustTalked = false;
        }
    }
}