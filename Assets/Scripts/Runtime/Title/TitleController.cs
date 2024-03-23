using UnityEngine;
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH1.Title
{
    public class TitleController : MonoBehaviour
    {
        private void Start()
        {
            Managers.Sound.Play(Sound.BGM, "Title_BGM_01");
        }
    }
}