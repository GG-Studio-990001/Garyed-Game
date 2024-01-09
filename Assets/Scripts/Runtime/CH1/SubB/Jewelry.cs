using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.SubB
{
    public class Jewelry : MonoBehaviour
    {
        [SerializeField] private Transform spriteTransform;
        [SerializeField] private float moveTime = 1.0f;
        
        private IMovement _movement;

        private void Start()
        {
            _movement = new JewelryMovement(this.transform, spriteTransform, moveTime);
        }
        
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Vector2 direction = (transform.position - collision.transform.position).normalized;
                direction.x = Mathf.Abs(direction.x) > Mathf.Abs(direction.y) ? Mathf.Sign(direction.x) : 0f;
                direction.y = Mathf.Abs(direction.y) > Mathf.Abs(direction.x) ? Mathf.Sign(direction.y) : 0f;

                _movement.Move(direction);
            }
        }
    }
}
