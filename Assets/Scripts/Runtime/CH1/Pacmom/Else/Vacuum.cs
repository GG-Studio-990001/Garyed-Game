using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Vacuum : MonoBehaviour
    {
        public PMController GameController;

        private void Eaten()
        {
            gameObject.SetActive(false);

            if (GameController != null)
                GameController.UseVacuum();
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