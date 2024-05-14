using UnityEngine;

namespace Runtime.CH1.Main
{
    public class BridgeController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _bridges;

        public void CheckBridge()
        {
            if (Managers.Data.Scene >= 3)
            {
                ActiveBrokenBridge();
            }
            else
            {
                ActiveNormalBridge();
            }
        }

        private void ActiveNormalBridge()
        {
            _bridges[0].SetActive(true);
            _bridges[1].SetActive(false);
        }

        public void ActiveBrokenBridge()
        {
            _bridges[0].SetActive(false);
            _bridges[1].SetActive(true);
        }
    }
}