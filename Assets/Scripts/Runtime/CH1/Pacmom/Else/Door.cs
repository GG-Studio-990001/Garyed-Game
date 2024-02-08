using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private GameObject door;
        [SerializeField]
        private GameObject doorStep;

        private void Update()
        {
            doorStep.SetActive(!door.activeSelf);
        }
    }
}