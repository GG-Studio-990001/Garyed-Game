using UnityEngine;

namespace Runtime.CH1.Main
{
    public class MamagoController : MonoBehaviour
    {
        public void CheckMamago()
        {
            if (Managers.Data.Scene <= 4)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}