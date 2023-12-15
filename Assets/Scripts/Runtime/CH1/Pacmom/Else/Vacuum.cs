using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Vacuum : MonoBehaviour
    {
        public PMGameController gameController;

        private void Eaten()
        {
            gameObject.SetActive(false);
            gameController?.UseVacuum();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PacmomStr))
            {
                Eaten();
            }
        }
    }
}