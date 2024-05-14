using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class LuckyPack : MonoBehaviour
    {
        public void ActiveLuckyPack()
        {
            this.gameObject.SetActive(!Managers.Data.MeetLucky);
        }
    }
}