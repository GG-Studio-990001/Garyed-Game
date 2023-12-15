using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Coin : MonoBehaviour
    {
        public PMGameController gameController;

        private void EatenByRapley()
        {
            gameObject.SetActive(false);
            gameController?.CoinEaten(GlobalConst.PlayerStr);
        }

        private void EatenByPacmom()
        {
            gameObject.SetActive(false);
            gameController?.CoinEaten(GlobalConst.PacmomStr);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                EatenByRapley();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PacmomStr))
            {
                EatenByPacmom();
            }
        }
    }
}