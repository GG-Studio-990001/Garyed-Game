using DG.Tweening;
using UnityEngine;

namespace Runtime.CH1.SubB
{
    // TODO 임시 코드 전부 리팩터링
    // 관리자로 두고 Board로 관리
    // 보드에서 매치가 되면 삭제, 움직이려는 곳에 존재한다면 움직이지 않음 등
    public class MoveJewelry : MonoBehaviour
    {
        public float pushForce = 1.0f;

        private Rigidbody2D rb;
        private bool isMoving = false;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !isMoving)
            {
                Vector2 direction = (transform.position - collision.transform.position).normalized;
                
                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    direction.y = 0f;
                    direction.x = Mathf.Sign(direction.x);
                }
                else
                {
                    direction.x = 0f;
                    direction.y = Mathf.Sign(direction.y);
                }

                Vector3 targetPosition = transform.position + (Vector3)direction;
                transform.DOMove(targetPosition, 1f)
                    .OnComplete(() => isMoving = false);

                isMoving = true;
            }
        }
    }
}
