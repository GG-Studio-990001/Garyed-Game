using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movement : MonoBehaviour
    {
        public Rigidbody2D Rigid { get; private set; }
        public Vector2 Direction { get; private set; }
        public Vector2 NextDirection { get; private set; }
        public Vector3 StartPosition { get; private set; }
        public bool CanMove { get; private set; }

        [SerializeField]
        private float _speed = 8f;
        [SerializeField]
        private float _speedMultiplier = 1f;
        [SerializeField]
        private Vector2 _initialDirection;
        private LayerMask _obstacleLayer;

        private void Update()
        {
            if (NextDirection != Vector2.zero || !CanMove)
            {
                SetDirection(NextDirection);
            }
        }

        protected void SetWhenAwake()
        {
            SetRigidBody(GetComponent<Rigidbody2D>());
            StartPosition = transform.position;
            CanMove = true;
            _obstacleLayer = LayerMask.GetMask(GlobalConst.ObstacleStr);
        }

        public virtual void ResetState()
        {
            transform.position = StartPosition;
            Direction = _initialDirection;
            NextDirection = Vector2.zero;
        }

        public void Move()
        {
            // 길 너비와 몸 지름이 같기 때문에 rigidbody를 주체로 움직임
            Vector2 position = Rigid.position;
            Vector2 translation =  _speed * _speedMultiplier * Time.fixedDeltaTime * Direction;

            Rigid.MovePosition(position + translation);
        }

        #region Set
        public void SetRigidBody(Rigidbody2D rigid)
        {
            Rigid = rigid;
        }

        public void SetSpeedMultiplier(float speedMultiplier)
        {
            _speedMultiplier = speedMultiplier;
        }

        public void SetCanMove(bool canMove)
        {
            CanMove = canMove;

            if (!CanMove)
                SetNextDirection(Vector2.zero);
        }
        #endregion

        #region Direction
        public void SetNextDirection(Vector2 direction)
        {
            NextDirection = direction;
        }

        protected virtual void SetDirection(Vector2 direction)
        {
            if (!CheckRoadBlocked(direction))
            {
                Direction = direction;
                NextDirection = Vector2.zero;
            }
        }

        public bool CheckRoadBlocked(Vector2 direction)
        {
            // 방향에 길이 막혀있으면 true 반환
            // 몸집이 있기 때문에 box로 검출
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1.0f, _obstacleLayer);

            return hit.collider != null;
        }
        #endregion
    }
}