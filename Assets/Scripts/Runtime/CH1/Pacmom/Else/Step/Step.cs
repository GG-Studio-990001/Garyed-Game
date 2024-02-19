using Runtime.ETC;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Step : MonoBehaviour
    {
        private LayerMask obstacleLayer;

        [field:SerializeField]
        public List<Vector2> availableDirections { get; private set; }

        private void Start()
        {
            obstacleLayer = LayerMask.GetMask(GlobalConst.ObstacleStr);

            availableDirections = new List<Vector2>();

            CheckAvailableDirection(Vector2.up);
            CheckAvailableDirection(Vector2.down);
            CheckAvailableDirection(Vector2.left);
            CheckAvailableDirection(Vector2.right);
        }

        private void CheckAvailableDirection(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, Vector2.one * 0.5f, 0f, direction, 1.5f, obstacleLayer);

            if (hit.collider == null)
            {
                availableDirections.Add(direction);
            }
        }

        public void AddStepsTwice(Step step, Vector2 direction)
        {
            for (int i = 0; i < 2; i++)
                step.availableDirections.Add(direction);
            // 구현 상속 대신 합성 사용
        }
    }
}