using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private GameObject door;

        public void ActiveDoor(bool isOpen)
        {
            door.SetActive(isOpen);
        }
    }
}