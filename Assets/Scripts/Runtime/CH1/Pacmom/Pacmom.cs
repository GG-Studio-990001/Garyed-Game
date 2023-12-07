using Runtime.ETC;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Pacmom : MonoBehaviour
    {
        public PacmomGameController gameController;
        public Movement movement;
        public bool isVacuumMode = false;

        private SpriteAnimation spriteAnim;
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private Sprite vacuumSpr;
        [SerializeField]
        private Sprite[] dieSpr;

        [SerializeField]
        private Transform[] enemys;

        private void Awake()
        {
            spriteAnim = GetComponent<SpriteAnimation>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            movement.spriteRotation.canRotate = true;
            movement.spriteRotation.canFlip = true;

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

        public void VacuumMode(bool mode)
        {
            SetRotateToZero();

            isVacuumMode = mode;
            movement.spriteRotation.canRotate = !isVacuumMode;

            if (isVacuumMode)
            {
                spriteAnim.enabled = false;
                spriteRenderer.sprite = vacuumSpr;
            }
            else
            {
                spriteAnim.enabled = true;
                spriteAnim.RestartAnim();
            }
        }

        public void PacmomDead()
        {
            StartCoroutine("DeadAnim");
        }

        private IEnumerator DeadAnim()
        {
            SetRotateToZero();

            spriteAnim.enabled = false;

            movement.speed = 0;
            movement.enabled = false;

            for (int i=0; i<dieSpr.Length; i++)
            {
                spriteRenderer.sprite = dieSpr[i];
                yield return new WaitForSeconds(0.3f);
            }
        }

        private void SetRotateToZero()
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Step step = other.GetComponent<Step>();

            if (step == null)
                return;

            Vector2 direction = Vector2.zero;

            foreach (Transform enemy in enemys)
            {
                // TO DO: 적 여러마리일 때 가장 가까운 적을 기준으로 구현 (먼지유령 추가 후)
                float distance = (enemy.position - transform.position).sqrMagnitude;

                if (distance < 30f)
                {
                    if (!isVacuumMode)
                    {
                        RunAwayFromEnemy(enemy, step);
                    }
                    else
                    {
                        ChaseEnemy(enemy, step);
                    }
                }
            }

            // TO DO: 적이 가까이 있지 않을 때 가까운 코인 감지

            if (direction == Vector2.zero) // 랜덤 이동
            {
                Debug.Log("아무데나!");

                int index = Random.Range(0, step.availableDirections.Count);

                if (step.availableDirections[index] == -1 * movement.direction && step.availableDirections.Count > 1)
                {
                    index++;

                    if (index >= step.availableDirections.Count)
                        index = 0;
                }
                direction = step.availableDirections[index];
            }

            movement.SetNextDirection(direction);
        }

        private Vector2 RunAwayFromEnemy(Transform enemy, Step step)
        {
            Debug.Log("도망가!");

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
            Debug.Log("쫓아가!");
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                if (isVacuumMode)
                {
                    gameController.RapleyEaten();
                }
                else
                {
                    gameController.PacmomEaten();
                }
            }
        }
    }
}
