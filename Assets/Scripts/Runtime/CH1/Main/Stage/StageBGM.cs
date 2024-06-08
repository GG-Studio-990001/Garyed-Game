using UnityEngine;
using Runtime.ETC;

namespace Runtime.CH1.Main.Stage
{
    public class StageBGM : MonoBehaviour
    {
        [SerializeField] private int _stageNum;

        private void OnEnable()
        {
            switch (_stageNum)
            {
                case 1:
                case 2:
                case 3:
                    Managers.Sound.Play(Sound.BGM, "[Ch1] Main_BGM", true);
                    break;
                case 4:
                    Managers.Sound.Play(Sound.BGM, "Mamago_BGM_1");
                    break;
                case 5:
                case 6:
                case 7:
                    Managers.Sound.Play(Sound.BGM, "[Ch1] Main(Cave)_BGM", true);
                    break;
            }
        }
    }
}