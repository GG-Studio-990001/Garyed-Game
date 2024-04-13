using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class AI : MonoBehaviour
    {
        public Movement Movement;
        public Transform[] Enemys;
        [SerializeField]
        private bool _doCoinMatter;
        [field: SerializeField]
        public bool IsStronger { get; private set; }
        private readonly float[] _roomPos = { -2.5f, 2.5f, -1.5f, 1f };

        public void SetAIStronger(bool isStronger)
        {
            IsStronger = isStronger;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<Step>(out _))
                return;

            Step step = other.GetComponent<Step>();

            float shortestDistance = float.MaxValue;
            Transform nearestEnemy = null;

            foreach (Transform enemy in Enemys)
            {
                float distanceFromEnemy = (enemy.position - transform.position).sqrMagnitude;

                if (distanceFromEnemy > 36f)
                    continue;

                if (_roomPos[0] < enemy.localPosition.x && enemy.localPosition.x < _roomPos[1] &&
                    _roomPos[2] < enemy.localPosition.y && enemy.localPosition.y < _roomPos[3])
                    continue;

                if (shortestDistance > distanceFromEnemy)
                {
                    shortestDistance = distanceFromEnemy;
                    nearestEnemy = enemy;
                }
            }

            Vector2 direction;

            if (nearestEnemy != null)
            {
                if (IsStronger)
                    direction = ChaseEnemy(nearestEnemy, step);
                else
                    direction = RunAwayFromEnemy(nearestEnemy, step);
            }
            else
            {
                if (_doCoinMatter)
                    direction = FindCoin(step);
                else
                    direction = MoveRandomly(step);
            }
            
            Movement.SetNextDirection(direction);
        }

        private Vector2 FindCoin(Step step)
        {
            if (step.AvailableDirections.Contains(Movement.Direction)) // 가던 방향에 코인이 있으면 그대로
            {
                if (DetectCoin(Movement.Direction))
                    return Movement.Direction;
            }

            foreach (Vector2 availableDirection in step.AvailableDirections)
            {
                if (availableDirection == Movement.Direction || availableDirection == -1 * Movement.Direction)
                    continue;

                if (DetectCoin(availableDirection))
                    return availableDirection;
            }

            return MoveRandomly(step);
        }

        private bool DetectCoin(Vector2 direction)
        {
            // 몸의 중심이 아닌 입을 기준으로
            Vector3 newPos = new(transform.position.x + direction.x * 2, transform.position.y + direction.y * 2, transform.position.z);
            RaycastHit2D hit = Physics2D.BoxCast(newPos, Vector2.one * 2f, 0, direction, 3f, LayerMask.GetMask("Coin"));

            return hit.collider != null;
        }

        private Vector2 MoveRandomly(Step step)
        {
            int index = Random.Range(0, step.AvailableDirections.Count);

            if (step.AvailableDirections[index] == -1 * Movement.Direction && step.AvailableDirections.Count > 1)
            {
                index++;

                if (index >= step.AvailableDirections.Count)
                    index = 0;
            }

            return step.AvailableDirections[index];
        }

        private Vector2 RunAwayFromEnemy(Transform enemy, Step step)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDirection in step.AvailableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
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

            foreach (Vector2 availableDirection in step.AvailableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
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