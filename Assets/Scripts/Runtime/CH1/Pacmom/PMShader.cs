using UnityEngine;
using UnityEngine.Rendering;

namespace Runtime.CH1.Pacmom
{
    public class PMShader : MonoBehaviour
    {
        [SerializeField]
        public Volume volume;
        private Bleed m_bleed;

        private void Start()
        {
            if (volume == null)
                return;

            volume.profile.TryGet(out m_bleed);

            if (m_bleed is null)
            {
                Debug.Log("Add Glitch1 effect to your Volume component to make Manipulation Example work");
                return;
            }

            m_bleed.active = true;
        }

        public void ChangeBleedAmount()
        {
            //Null check
            if (volume == null)
                return;
            if (m_bleed is null)
                return;

            m_bleed.bleedAmount.value = 10f;
        }
    }
}
