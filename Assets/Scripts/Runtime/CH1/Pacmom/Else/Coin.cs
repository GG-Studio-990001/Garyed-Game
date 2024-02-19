using Runtime.ETC;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Coin : MonoBehaviour
    {
        public PMGameController gameController;
        public Vector3 defaultPos;

        private void Start()
        {
            defaultPos = transform.position;
        }

        public void ResetCoin()
        {
            transform.position = defaultPos;
            gameObject.SetActive(true);
            // SetActive를 Coin 스크립트로 옮긴건 좋은 것 같음 (자율적인 객체)
        }

        private void EatenByRapley()
        {
            gameObject.SetActive(false);
            gameController?.CoinEatenByRapley();
        }

        private void EatenByPacmom()
        {
            gameObject.SetActive(false);
            gameController?.CoinEatenByPacmom();
        }

        private void SuckByVacuum()
        {
            StartCoroutine("SuckCoin");
        }

        private IEnumerator SuckCoin()
        {
            float duration = 0.2f;
            float elapsed = 0.0f;

            while (elapsed < duration)
            {
                Vector3 pacomPos = gameController.GetPacmomPos();
                Vector3 newPosition = Vector3.Lerp(defaultPos, pacomPos, elapsed / duration);
                newPosition.z = transform.position.z;
                this.transform.position = newPosition;

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PlayerStr))
            {
                EatenByRapley();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer(GlobalConst.PacmomStr))
            {
                if (other.gameObject.tag == GlobalConst.VacuumStr)
                {
                    SuckByVacuum();
                }
                else
                {
                    EatenByPacmom();
                }
            }
        }
    }
}