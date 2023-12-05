using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Coin : MonoBehaviour
    {
        public PacmomGameController gameController;

        private void EatenByRapley()
        {
            gameController.CoinEaten(this, GlobalConst.PlayerStr);
        }

        private void EatenByPacmom()
        {
            gameController.CoinEaten(this, GlobalConst.PacmomStr);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                EatenByRapley();
            }
            else if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PacmomStr))
            {
                EatenByPacmom();
            }

        }
    }
}