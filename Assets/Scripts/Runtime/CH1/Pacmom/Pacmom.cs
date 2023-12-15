using Runtime.ETC;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Pacmom : MonoBehaviour
    {
        public PMGameController gameController;
        public MovementAndRotation movement;
        public AI ai;

        private void Start()
        {
            movement.spriteRotation.canRotate = true;
            movement.spriteRotation.canFlip = true;

            ResetState();
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
            movement.spriteRotation.canRotate = !isVacuum;
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
