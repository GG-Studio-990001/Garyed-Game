using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Coin : MonoBehaviour
    {
        private void EatenByRapley()
        {
            FindObjectOfType<PacmomGameController>().CoinEaten(this, "Rapley");
        }

        private void EatenByPacmom()
        {
            FindObjectOfType<PacmomGameController>().CoinEaten(this, "Pacmom");
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                EatenByRapley();
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Pacmom"))
            {
                EatenByPacmom();
            }

        }
    }
}