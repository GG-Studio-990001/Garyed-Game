using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Dust : MonoBehaviour
    {
        public Movement movement;

        private void Start()
        {
            movement.isRotationNeeded = false;
            ResetState();
        }

        public void ResetState()
        {
            movement.ResetState();
        }

        private void FixedUpdate()
        {
            movement.Move();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Step step = other.GetComponent<Step>();

            if (step == null)
                return;

            Vector2 direction = Vector2.zero;

            int index = Random.Range(0, step.availableDirections.Count);

            if (step.availableDirections[index] == -1 * movement.direction && step.availableDirections.Count > 1)
            {
                index++;

                if (index >= step.availableDirections.Count)
                    index = 0;
            }
            direction = step.availableDirections[index];

            movement.SetNextDirection(direction);
        }
    }
}