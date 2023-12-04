using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Passage : MonoBehaviour
    {
        public Transform connection;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Vector3 position = other.transform.position;
            position.x = connection.position.x;
            position.y = connection.position.y;

            other.transform.position = position;
        }
    }
}