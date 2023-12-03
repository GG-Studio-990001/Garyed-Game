using Runtime.CH1.Main;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Pacmom : MonoBehaviour
    {
        public Movement movement;

        private void Start()
        {
            ResetState();
        }

        public void ResetState()
        {
            if (gameObject.activeInHierarchy == false)
            {
                gameObject.SetActive(true);
            }
            movement.ResetState();
        }

        private void FixedUpdate()
        {
            movement.Move();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Step step = other.GetComponent<Step>();

            if (step != null)
            {
                int index = Random.Range(0, step.availableDirections.Count);

                // 거꾸로 가기 방지
                if (step.availableDirections[index] == -1 * movement.direction && step.availableDirections.Count > 1)
                {
                    index++; // 랜덤으로 대신 +1

                    if (index >= step.availableDirections.Count)
                        index = 0;
                }

                movement.SetNextDirection(step.availableDirections[index]);
            }
        }
    }
}
