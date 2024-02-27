using Runtime.ETC;
using Runtime.Interface.Pacmom;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Dust : MonoBehaviour, ICharacter, IFoodChain
    {
        public PMGameController gameController;
        public MovementAndEyes movement { get; set; }
        private AI ai;
        [field:SerializeField]
        public int dustID { get; private set; }

        private void Awake()
        {
            movement = GetComponent<MovementAndEyes>();
            ai = GetComponent<AI>();
        }

        private void Start()
        {
            SetStronger(true);
            ResetState();
        }

        public bool IsStronger()
        {
            return ai.isStronger;
        }

        public void SetStronger(bool isStrong)
        {
            ai?.SetAIStronger(isStrong);
        }

        public void ResetState()
        {
            movement.SetEyeNormal(true);
            movement.GetEyeSpriteByPosition();
            movement.ResetState();
        }

        private void FixedUpdate()
        {
            movement.Move();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PacmomStr))
            {
                if (ai.isStronger)
                {
                    if (collision.gameObject.tag != GlobalConst.VacuumStr)
                    {
                        gameController?.PacmomEatenByDust(dustID);
                    }
                }
                else
                {
                    gameController?.DustEaten(this);
                }
            }
        }
    }
}