using Runtime.ETC;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Room : MonoBehaviour
    {
        [SerializeField]
        private MovementAndEyes movement;
        [SerializeField]
        private Transform inside;
        [SerializeField]
        private Transform outside;
        public bool isInRoom = true;

        public void ExitRoom(float afterTime)
        {
            Invoke("DoExitTransition", afterTime);
        }

        private void DoExitTransition()
        {
            StopAllCoroutines();
            StartCoroutine(ExitTransition());
        }

        private IEnumerator ExitTransition()
        {
            movement.rigid.isKinematic = true;
            movement.enabled = false;

            Vector3 position = this.transform.position;

            float duration = 0.5f;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                Vector3 newPosition = Vector3.Lerp(position, this.inside.position, elapsed / duration);
                newPosition.z = position.z;
                movement.rigid.position = newPosition;
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0.0f;
            movement.GetEyeSprites(Vector2.up);

            while (elapsed < duration)
            {
                Vector3 newPosition = Vector3.Lerp(this.inside.position, this.outside.position, elapsed / duration);
                newPosition.z = position.z;
                movement.rigid.position = newPosition;
                elapsed += Time.deltaTime;
                yield return null;
            }

            movement.SetNextDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f));
            movement.rigid.isKinematic = false;
            movement.enabled = true;

            isInRoom = false;
        }
    }
}