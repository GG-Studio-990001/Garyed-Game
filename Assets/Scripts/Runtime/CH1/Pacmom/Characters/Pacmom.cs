using Runtime.ETC;
using Runtime.Interface.Pacmom;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Pacmom : MonoBehaviour, ICharacter, IFoodChain
    {
        public PMGameController GameController;
        public MovementAndRotation Movement { get; set; }
        private AI _ai;
        [SerializeField]
        private GameObject _vacuum;

        private void Awake()
        {
            Movement = GetComponent<MovementAndRotation>();
            _ai = GetComponent<AI>();
        }

        private void Start()
        {
            SetSpriteRotation();
            SetStronger(false);
            ResetState();
        }

        public void SetStronger(bool isStrong)
        {
            _ai?.SetAIStronger(isStrong);
        }

        private void SetSpriteRotation()
        {
            Movement.SpriteRotation.SetCanRotate(true);
            Movement.SpriteRotation.SetCanFlip(true);
        }

        public void ResetState()
        {
            SetRotateToZero();
            Movement.ResetState();
        }

        private void FixedUpdate()
        {
            Movement.Move();
        }

        public void VacuumMode(bool isVacuum)
        {
            SetRotateToZero();

            _ai.SetAIStronger(isVacuum);
            Movement.SpriteRotation.SetCanRotate(!isVacuum);
            _vacuum.SetActive(isVacuum);
        }

        public void SetRotateToZero()
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                if (_ai.IsStronger)
                    GameController?.RapleyEaten();
                else
                    GameController?.PacmomEatenByRapley();
            }
        }
    }
}
