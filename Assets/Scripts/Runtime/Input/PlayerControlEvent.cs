using Runtime.InGameSystem;
using UnityEngine;

namespace Runtime.CH1.Main
{
    public class PlayerControlEvent : MonoBehaviour
    {
        public void RestrictInput()
        {
            DataProviderManager.Instance.ControlsDataProvider.Get().RestrictPlayerInput();
        }
        
        public void ReleaseInput()
        {
            DataProviderManager.Instance.ControlsDataProvider.Get().ReleasePlayerInput();
        }
    }
}