using Runtime.ETC;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Pacmom : MonoBehaviour
    {
        public PacmomGameController gameController;
        public Movement movement;
        public PacmomSpriteController spriteControl;
        public bool isVacuumMode = false;

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

        public void VacuumMode(bool mode)
        {
            SetRotateToZero();

            isVacuumMode = mode;
            movement.spriteRotation.canRotate = !isVacuumMode;

            if (isVacuumMode)
            {
                spriteControl.GetVacuumModeSprite();
            }
            else
            {
                spriteControl.GetNormalSprite();
            }
        }

        public void VacuumModeAlmostOver()
        {
            spriteControl.GetVaccumBlinkSprite();
        }

        public void PacmomDead()
        {
            SetRotateToZero();
            spriteControl.GetDieSprite();
        }

        private void SetRotateToZero()
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                if (isVacuumMode)
                {
                    gameController.RapleyEaten();
                }
                else
                {
                    gameController.PacmomEaten();
                }
            }
        }
    }
}
