using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.CH1.Pacmom
{
    public class PMShader : MonoBehaviour
    {
        [SerializeField]
        private Volume _volume;
        private Bleed _bleed;

        private void Start()
        {
            if (_volume == null)
                return;

            _volume.profile.TryGet(out _bleed);

            if (_bleed == null)
            {
                Debug.Log("Add Glitch1 effect to your Volume component to make Manipulation Example work");
                return;
            }

            _bleed.active = true;
        }

        public void ChangeBleedAmount()
        {
            if (_volume == null || _bleed == null)
                return;

            _bleed.bleedAmount.value = 10f;
        }
    }
}