using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class MovementWithEyes : Movement
    {
        [SerializeField]
        private SpriteRenderer _eyeSprite;
        [SerializeField]
        private Sprite[] _EyeSprites;
        [field:SerializeField]
        public bool IsNormalEye { get; private set; }

        private void Awake()
        {
            SetWhenAwake();
        }

        public override void ResetState()
        {
            base.ResetState();

            GetEyeSpriteByPosition();
        }

        protected override void SetDirection(Vector2 direction)
        {
            base.SetDirection(direction);

            if (!CheckRoadBlocked(direction))
            {
                GetEyeSprites(direction);
            }
        }

        public void SetEyeNormal(bool isNormalEye)
        {
            IsNormalEye = isNormalEye;
        }

        public void GetEyeSpriteByPosition()
        {
            GetEyeSprites(transform.localPosition.x < 0 ? Vector2.right : Vector2.left);
        }

        public void GetEyeSprites(Vector2 direction)
        {
            if (_eyeSprite == null || !IsNormalEye)
                return;

            int idx = 0;

            switch (direction)
            {
                case Vector2 v when v.Equals(Vector2.up):
                    idx = 0;
                    break;
                case Vector2 v when v.Equals(Vector2.down):
                    idx = 1;
                    break;
                case Vector2 v when v.Equals(Vector2.right):
                    idx = 2;
                    break;
                case Vector2 v when v.Equals(Vector2.left):
                    idx = 3;
                    break;
            }

            _eyeSprite.sprite = _EyeSprites[idx];
        }
    }
}