using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class DustRoom : MonoBehaviour
    {
        private MovementAndEyes _movement;
        [SerializeField]
        private Transform _inside;
        [SerializeField]
        private Transform _outside;
        [SerializeField]
        private Transform _crossRoad;
        public bool IsInRoom { get; private set; }

        private void Awake()
        {
            _movement = GetComponent<MovementAndEyes>();
        }

        public void SetInRoom(bool isInRoom)
        {
            IsInRoom = isInRoom;
        }

        public void ExitRoom(float afterTime)
        {
            StartCoroutine(nameof(ExitTransition), afterTime);
        }

        private void Transition(Vector3 start, Vector3 end, float lerpTime)
        {
            Vector3 newPosition = Vector3.Lerp(start, end, lerpTime);
            newPosition.z = transform.position.z;
            _movement.Rigid.position = newPosition;
        }

        private IEnumerator ExitTransition(float afterTime)
        {
            Vector3 position = transform.position;

            _movement.GetEyeSpriteByPosition();
            _movement.Rigid.isKinematic = true;
            _movement.enabled = false;

            yield return new WaitForSeconds(afterTime);

            float duration = 0.5f;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                Transition(position, _inside.position, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            elapsed = 0.0f;
            _movement.GetEyeSprites(Vector2.up);

            while (elapsed < duration)
            {
                Transition(_inside.position, _outside.position, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            duration = 0.3f;
            elapsed = 0.0f;

            Vector2 crossRoadDirection = (_crossRoad.position.x < 0 ? Vector2.left : Vector2.right);
            _movement.GetEyeSprites(crossRoadDirection);

            while (elapsed < duration)
            {
                Transition(_outside.position, _crossRoad.position, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            _movement.SetNextDirection(crossRoadDirection);
            _movement.Rigid.isKinematic = false;
            _movement.enabled = true;

            _movement.SetCanMove(true);
            IsInRoom = false;
        }
    }
}