using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Dust : MonoBehaviour
    {
        public PacmomGameController gameController;
        public Movement movement;

        private void Start()
        {
            movement.isRotationNeeded = false;
            ResetState();
        }

        public void ResetState()
        {
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
                gameController?.PacmomDustCollision(this);
            }
        }
    }
}