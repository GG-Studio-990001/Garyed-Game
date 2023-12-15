using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class AI : MonoBehaviour
    {
        public Movement movement;
        public Transform[] enemys;
        public bool isStronger { get; private set; }

        public void SetStronger(bool isStronger)
        {
            this.isStronger = isStronger;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Step step = other.GetComponent<Step>();

            if (step == null)
                return;

            Vector2 direction = Vector2.zero;
            float distance = float.MaxValue;
            Transform closeEnemy = null;

            // 가장 가까운 적 찾기
            foreach (Transform enemy in enemys)
            {
                float minDistance = (enemy.position - transform.position).sqrMagnitude;

                if (distance > minDistance)
                {
                    distance = minDistance;
                    closeEnemy = enemy;
                }
            }

            // 가장 가까운 적이 특정 거리 이내이면 도망가거나 쫓음
            if (closeEnemy != null && distance <= 36f)
            {
                if (isStronger)
                    direction = ChaseEnemy(closeEnemy, step);
                else
                    direction = RunAwayFromEnemy(closeEnemy, step);
            }
            else
                direction = GetRandomDirection(step);

            movement.SetNextDirection(direction);
        }

        private Vector2 GetRandomDirection(Step step)
        {
            int index = Random.Range(0, step.availableDirections.Count);

            if (step.availableDirections[index] == -1 * movement.direction && step.availableDirections.Count > 1)
            {
                index++;

                if (index >= step.availableDirections.Count)
                    index = 0;
            }

            return step.availableDirections[index];
        }

        private Vector2 RunAwayFromEnemy(Transform enemy, Step step)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDirection in step.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float newDistance = (enemy.position - newPosition).sqrMagnitude;

                if (newDistance > maxDistance)
                {
                    maxDistance = newDistance;
                    direction = availableDirection;
                }
            }

            return direction;
        }

        private Vector2 ChaseEnemy(Transform enemy, Step step)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            foreach (Vector2 availableDirection in step.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float newDistance = (enemy.position - newPosition).sqrMagnitude;

                if (newDistance < minDistance)
                {
                    minDistance = newDistance;
                    direction = availableDirection;
                }
            }

            return direction;
        }
    }
}