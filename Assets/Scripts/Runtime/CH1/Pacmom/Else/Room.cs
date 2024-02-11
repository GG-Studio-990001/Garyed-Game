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
        [SerializeField]
        private Transform crossRoad;
        public bool isInRoom { get; private set; }

        public void SetInRoom(bool isInRoom)
        {
            this.isInRoom = isInRoom;
        }

        public void ExitRoom(float afterTime)
        {
            StartCoroutine("ExitTransition", afterTime);
        }

        private void Transition(Vector3 start, Vector3 end, float lerpTime)
        {
            Vector3 position = transform.position;
            Vector3 newPosition = Vector3.Lerp(start, end, lerpTime);
            newPosition.z = position.z;
            movement.rigid.position = newPosition;
        }

        private IEnumerator ExitTransition(float afterTime)
        {
            Vector3 position = transform.position;

            movement.GetEyeSpriteByPosition();
            movement.rigid.isKinematic = true;
            movement.enabled = false;

            yield return new WaitForSeconds(afterTime);

            float duration = 0.5f;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                Transition(position, inside.position, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0.0f;
            movement.GetEyeSprites(Vector2.up);

            while (elapsed < duration)
            {
                Transition(inside.position, outside.position, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            duration = 0.3f;
            elapsed = 0.0f;
            movement.GetEyeSprites(new Vector2(crossRoad.position.x < 0 ? -1.0f : 1.0f, 0.0f));

            while (elapsed < duration)
            {
                Transition(outside.position, crossRoad.position, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            movement.SetNextDirection(new Vector2(crossRoad.position.x < 0 ? -1.0f : 1.0f, 0.0f));
            movement.rigid.isKinematic = false;
            movement.enabled = true;

            movement.SetCanMove(true);
            isInRoom = false;
        }
    }
}