using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Coin : MonoBehaviour
    {
        private void Eaten()
        {
            FindObjectOfType<PacmomGameController>().CoinEaten(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Eaten();
            }
        }
    }
}