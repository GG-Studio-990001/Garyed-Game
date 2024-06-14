using UnityEngine;
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH2
{
    public class CH2Controller : MonoBehaviour
    {
        void Start()
        {
            Managers.Sound.Play(Sound.BGM, "[Ch1] Lucky_BGM_3");
        }
    }
}