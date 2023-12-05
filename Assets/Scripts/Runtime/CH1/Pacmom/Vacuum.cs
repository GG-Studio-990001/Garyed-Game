using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Vacuum : MonoBehaviour
    {
        private void Eaten()
        {
            FindObjectOfType<PacmomGameController>().UseVacuum(this);
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