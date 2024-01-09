using DG.Tweening;
using Runtime.CH1.Main.PlayerFunction;
using Runtime.Interface;
using System;
using UnityEngine;

namespace Runtime.CH1.SubB
{
    // TODO 임시 코드 전부 리팩터링
    // 관리자로 두고 Board로 관리
    // 보드에서 매치가 되면 삭제, 움직이려는 곳에 존재한다면 움직이지 않음 등
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

                _movement.Move(direction);
            }
        }
    }
}
