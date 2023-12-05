using Runtime.CH1.Main;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Pacmom : MonoBehaviour
    {
        public Movement movement;
        public bool isVacuumMode = false;

        [SerializeField]
        private Sprite vacuumSpr;
        [SerializeField]
        private Sprite[] dieSpr;

        private void Awake()
        {
            movement.canFlip = true;
            movement.canUpDown = true;
        }

        private void Start()
        {
            ResetState();
        }

        public void ResetState()
        {
            if (gameObject.activeInHierarchy == false)
            {
                gameObject.SetActive(true);
            }
            movement.ResetState();
        }

        private void FixedUpdate()
        {
            movement.Move();
        }

        public void VacuumMode(bool mode)
        {
            isVacuumMode = mode;
            movement.canUpDown = !isVacuumMode;

            SpriteAnimation spriteAnim = gameObject.GetComponent<SpriteAnimation>();
            SpriteRenderer spriteRender = gameObject.GetComponent<SpriteRenderer>();

            if (isVacuumMode)
            {
                spriteAnim.enabled = false;
                spriteRender.sprite = vacuumSpr;
            }
            else
            {
                spriteAnim.enabled = true;
                spriteAnim.RestartAnim();
            }
        }

        public void PacmomDead()
        {
            StartCoroutine("DeadAnim");
        }

        private IEnumerator DeadAnim()
        {
            SpriteRenderer spriteRender = gameObject.GetComponent<SpriteRenderer>();
            SpriteAnimation spriteAnim = gameObject.GetComponent<SpriteAnimation>();
            spriteAnim.enabled = false;

            movement.speed = 0;
            movement.enabled = false;

            for (int i=0; i<dieSpr.Length; i++)
            {
                spriteRender.sprite = dieSpr[i];
                yield return new WaitForSeconds(0.3f);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Step step = other.GetComponent<Step>();

            if (step != null)
            {
                int index = Random.Range(0, step.availableDirections.Count);

                // 거꾸로 가기 방지
                if (step.availableDirections[index] == -1 * movement.direction && step.availableDirections.Count > 1)
                {
                    index++; // 랜덤으로 대신 +1

                    if (index >= step.availableDirections.Count)
                        index = 0;
                }

                movement.SetNextDirection(step.availableDirections[index]);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (isVacuumMode)
                {
                    FindObjectOfType<PacmomGameController>().RapleyEaten();
                }
                else
                {
                    FindObjectOfType<PacmomGameController>().PacmomEaten();
                }
            }
        }
    }
}
