using UnityEngine;
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH2.Main
{
    public class CH2Controller : MonoBehaviour
    {
        void Start()
        {
            Managers.Sound.Play(Sound.BGM, "Floyard_BGM_2");
            // Managers.Sound.Play(Sound.BGM, "[Ch1] Lucky_BGM_3");
        }
    }
}