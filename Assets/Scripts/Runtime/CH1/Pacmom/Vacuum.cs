using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Vacuum : MonoBehaviour
    {
        public PacmomGameController gameController;

        private void Eaten()
        {
            gameController.UseVacuum(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Pacmom"))
            {
                Eaten();
            }

        }
    }
}