using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class AI : MonoBehaviour
    {
        public Movement movement;
        public Transform[] enemys;
        public bool isStronger { get; private set; }
        public bool doCoinMatter { get; private set; }

        public void SetStronger(bool isStronger)
        {
            this.isStronger = isStronger;
        }

        public void SetCoinMatter(bool doCoinMatter)
        {
            this.doCoinMatter = doCoinMatter;
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

                // 멀면 X
                if (minDistance > 36f)
                    continue;

                // 방 안이면 X
                if (-2.5f < enemy.localPosition.x && enemy.localPosition.x < 2.5f &&
                    -1.5f < enemy.localPosition.y && enemy.localPosition.y < 0.5f)
                    continue;

                if (distance > minDistance)
                {
                    distance = minDistance;
                    closeEnemy = enemy;
                }
            }

            if (closeEnemy != null)
            {
                if (isStronger)
                    direction = ChaseEnemy(closeEnemy, step);
                else
                    direction = RunAwayFromEnemy(closeEnemy, step);
            }
            else
            {
                if (doCoinMatter)
                    direction = FindCoin(step);
                else
                    direction = MoveRandomly(step);
            }
            
            movement.SetNextDirection(direction);
        }

        private Vector2 FindCoin(Step step)
        {
            if (step.availableDirections.Contains(movement.direction)) // 가던 방향에 코인이 있으면 그대로
            {
                if (DetectCoin(movement.direction))
                    return movement.direction;
            }

            foreach (Vector2 availableDirection in step.availableDirections)
            {
                if (availableDirection == movement.direction || availableDirection == -1 * movement.direction)
                    continue;

                if (DetectCoin(availableDirection))
                    return availableDirection;
            }

            return MoveRandomly(step);
        }

        private bool DetectCoin(Vector2 direction)
        {
            // 몸의 중심이 아닌 입을 기준으로
            Vector3 newPos = new Vector3(transform.position.x + direction.x * 2, transform.position.y + direction.y * 2, transform.position.z);
            RaycastHit2D hit = Physics2D.BoxCast(newPos, Vector2.one * 2f, 0, direction, 3f, LayerMask.GetMask("Coin"));

            return hit.collider != null;
        }

        private Vector2 MoveRandomly(Step step)
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
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0f);
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
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y, 0f);
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