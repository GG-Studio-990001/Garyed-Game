using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.CH1.Pacmom
{
    public class PMShader : MonoBehaviour
    {
        [SerializeField]
        public Volume volume;
        private Bleed _m_bleed;

        private void Start()
        {
            if (volume is null)
                return;

            volume.profile.TryGet(out _m_bleed);

            if (_m_bleed is null)
            {
                Debug.Log("Add Glitch1 effect to your Volume component to make Manipulation Example work");
                return;
            }

            _m_bleed.active = true;
        }

        public void ChangeBleedAmount()
        {
            //Null check
            if (volume is null)
                return;
            if (_m_bleed is null)
                return;

            _m_bleed.bleedAmount.value = 10f;
        }
    }
}