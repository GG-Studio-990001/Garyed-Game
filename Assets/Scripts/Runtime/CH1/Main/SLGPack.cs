using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Main
{
    public class SLGPack : MonoBehaviour
    {
        public void ActiveLuckyPack()
        {
            gameObject.SetActive(Managers.Data.SLGProgressData == SLGProgress.None);
        }
    }
}