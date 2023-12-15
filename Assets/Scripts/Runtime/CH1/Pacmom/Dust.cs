using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Dust : MonoBehaviour
    {
        public PMGameController gameController;
        public MovementAndEyes movement;
        public AI ai;

        private void Start()
        {
            ai?.SetStronger(true);
            ResetState();
        }

        public void ResetState()
        {
            movement.GetEyeSprites(new Vector2(transform.position.x < 0 ? 1 : -1, 0));
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
                    gameController?.PacmomEaten(GlobalConst.DustStr);
                else
                    gameController?.DustEaten(this);
            }
        }
    }
}