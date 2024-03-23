using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Passage : MonoBehaviour
    {
        [SerializeField]
        private Transform _connection;

        private void OnTriggerEnter2D(Collider2D character)
        {
            Vector3 newPosition = new(_connection.position.x, _connection.position.y, character.transform.position.z);

            character.transform.position = newPosition;
        }
    }
}