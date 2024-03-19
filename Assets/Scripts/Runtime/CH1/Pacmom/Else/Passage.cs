using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Passage : MonoBehaviour
    {
        [SerializeField]
        private Transform connection;

        private void OnTriggerEnter2D(Collider2D character)
        {
            Vector3 newPosition = new Vector3(connection.position.x, connection.position.y, character.transform.position.z);

            character.transform.position = newPosition;
        }
    }
}