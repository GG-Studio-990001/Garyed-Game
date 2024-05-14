using UnityEngine;

namespace Runtime.CH1.Main
{
    public class LuckyPack : MonoBehaviour
    {
        public void ActiveLuckyPack()
        {
            this.gameObject.SetActive(!Managers.Data.MeetLucky);
        }
    }
}