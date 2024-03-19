using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Door : MonoBehaviour
    {
        [SerializeField]
        private GameObject _door;

        public void ActiveDoor(bool isOpen)
        {
            _door.SetActive(isOpen);
        }
    }
}