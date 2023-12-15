using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Coin : MonoBehaviour
    {
        public PMGameController gameController;

        private void EatenByRapley()
        {
            gameController?.CoinEaten(GlobalConst.PlayerStr);
        }

        private void EatenByPacmom()
        {
            gameController?.CoinEaten(GlobalConst.PacmomStr);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                gameObject.SetActive(false);
                EatenByRapley();
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PacmomStr))
            {
                gameObject.SetActive(false);
                EatenByPacmom();
            }

        }
    }
}