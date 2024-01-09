using DG.Tweening;
using Runtime.Interface;
using UnityEngine;

namespace Runtime.CH1.SubB
{
    public class JewelryMovement : IMovement
    {
        public Vector2 Direction => _previousMovementInput;
        
        private readonly Transform _transform;
        private readonly float _moveSpeed;
        private bool _isMoving = false;
        private Vector2 _previousMovementInput = Vector2.zero;
        private Transform _spriteTransform;
        
        public JewelryMovement(Transform transform, Transform spriteTransform, float moveSpeed)
        {
            _transform = transform;
            _spriteTransform = spriteTransform;
            _moveSpeed = moveSpeed;
        }
        
        public bool Move(Vector2 movementInput)
        {
            if (_isMoving)
            {
                return false;
            }
            
            _previousMovementInput = movementInput;
            
            Vector3 targetPosition = _transform.position + (Vector3)movementInput;
            
            Sequence sequence = DOTween.Sequence();

// DOMove를 시퀀스에 추가
            Tweener moveTween = _transform.DOMove(targetPosition, _moveSpeed).SetEase(Ease.Linear);
            sequence.Append(moveTween);

// DOShakePosition을 시퀀스에 추가하고 DOMove가 종료되면 중지
            Tweener shakeTween = _spriteTransform.DOShakePosition(1, new Vector3(0.01f, 0.01f, 0.01f), 10, 90F, false, false).SetLoops(-1);
            sequence.Insert(0, shakeTween); // DOMove 전에 실행

// DOMove가 완료되면 호출할 콜백 설정
            sequence.OnComplete(() =>
            {
                // DOMove가 완료되면 DOShakePosition을 중지
                shakeTween.Kill();
                // _isMoving을 false로 설정
                _isMoving = false;
            });

// 시퀀스를 시작
            sequence.Play();
            
            return true;
        }
    }
}