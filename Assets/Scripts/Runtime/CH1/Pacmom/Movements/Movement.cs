using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        public Rigidbody2D rigid;
        public Vector2 direction { get; private set; }
        public Vector2 nextDirection { get; private set; }
        public Vector3 startPosition { get; private set; }
        public bool canStop { get; private set; }

        [SerializeField]
        private float speed = 8f;
        [SerializeField]
        private float speedMultiplier = 1f;
        [SerializeField]
        private Vector2 initialDirection;

        private LayerMask obstacleLayer;

        private void Update()
        {
            if (nextDirection != Vector2.zero || canStop)
            {
                SetDirection(nextDirection);
            }
        }

        protected void Set()
        {
            // Awake에서 상속 필수
            rigid = GetComponent<Rigidbody2D>();
            startPosition = transform.position;
            canStop = false;
            obstacleLayer = LayerMask.GetMask(GlobalConst.ObstacleStr);
        }

        public virtual void ResetState()
        {
            transform.position = startPosition;
            direction = initialDirection;
            nextDirection = Vector2.zero;
        }

        public void Move()
        {
            // 길 너비와 몸 지름이 같기 때문에 rigidbody를 주체로 움직임
            Vector2 position = rigid.position;
            Vector2 translation = direction * speed * speedMultiplier * Time.fixedDeltaTime;

            rigid.MovePosition(position + translation);
        }

        public void SetSpeedMultiplier(float multiplier)
        {
            speedMultiplier = multiplier;
        }

        public void Stop()
        {
            SetNextDirection(Vector2.zero);
            canStop = true;
        }

        public void Resume()
        {
            canStop = false;
        }

        #region Direction
        public void SetNextDirection(Vector2 direction)
        {
            nextDirection = direction;
        }

        protected virtual void SetDirection(Vector2 direction)
        {
            if (!CheckRoadBlocked(direction))
            {
                this.direction = direction;
                nextDirection = Vector2.zero;
            }
        }

        public bool CheckRoadBlocked(Vector2 direction)
        {
            // 방향에 길이 막혀있으면 true 반환
            // 몸집이 있기 때문에 box로 검출
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1.0f, obstacleLayer);

            return hit.collider != null;
        }
        #endregion
    }
}
