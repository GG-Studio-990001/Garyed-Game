using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.LookDev;
using UnityEngine.Rendering.Universal;

namespace Runtime.CH1.Main.Dialogue
{
    public class Ch1MainVolumeController : MonoBehaviour
    {
        [SerializeField] private Volume volume;
        
        public void SetCinematicBars(float value)
        {
            CinematicBars cinematicBars;

            if (volume.profile.TryGet(out cinematicBars))
            {
                cinematicBars.amount.value = value;
            }
        }
    }
}
