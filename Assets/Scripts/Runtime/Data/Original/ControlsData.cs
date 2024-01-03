using UnityEngine;

namespace Runtime.Data.Original
{
    [CreateAssetMenu(fileName = "ControlsData", menuName = "ScriptableObject/ControlsData", order = 0)]
    public class ControlsData : ScriptableObject
    {
        public GameOverControls gameOverControls;
    }
}