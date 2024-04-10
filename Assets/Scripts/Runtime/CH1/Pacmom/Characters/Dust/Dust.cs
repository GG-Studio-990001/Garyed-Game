using Runtime.ETC;
using Runtime.Interface.Pacmom;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Dust : MonoBehaviour, ICharacter, IFoodChain
    {
        public PMGameController GameController;
        public MovementWithEyes Movement { get; set; }
        private AI _ai;
        [field:SerializeField]
        public int DustID { get; private set; }

        private void Awake()
        {
            Movement = GetComponent<MovementWithEyes>();
            _ai = GetComponent<AI>();
        }

        private void Start()
        {
            SetStronger(true);
            ResetState();
        }

        public bool IsStronger()
        {
            return _ai.IsStronger;
        }

        public void SetStronger(bool isStrong)
        {
            if (_ai != null)
                _ai.SetAIStronger(isStrong);
        }

        public void ResetState()
        {
            Movement.SetEyeNormal(true);
            Movement.GetEyeSpriteByPosition();
            Movement.ResetState();
        }

        private void FixedUpdate()
        {
            Movement.Move();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PacmomStr))
            {
                if (GameController == null)
                    return;

                if (_ai.IsStronger)
                {
                    if (collision.gameObject.CompareTag(GlobalConst.VacuumStr))
                        return;
                    
                    GameController.PacmomEatenByDust(DustID);
                }
                else
                {
                    GameController.DustEaten(this);
                }
            }
        }
    }
}