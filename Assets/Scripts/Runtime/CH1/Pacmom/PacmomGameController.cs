using DG.Tweening.Core.Easing;
using Runtime.ETC;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PacmomGameController : MonoBehaviour
    {

        [SerializeField]
        private Rapley rapley;
        private RapleySpriteControl rapleySprite;
        [SerializeField]
        private Pacmom pacmom;
        private PacmomSpriteControl pacmomSprite;
        [SerializeField]
        private DustSpriteControl[] dustSprite;
        [SerializeField]
        private Transform coins;
        [SerializeField]
        private Transform vacuums;
        private float vacuumDuration = 10f;
        private float vacuumEndDuration = 3f;

        public int rapleyScore { get; private set; }
        public int pacmomScore { get; private set; }
        public int pacmomLives { get; private set; }

        private void Awake()
        {
            rapleySprite = rapley.gameObject.GetComponent<RapleySpriteControl>();
            pacmomSprite = pacmom.gameObject.GetComponent<PacmomSpriteControl>();
        }

        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            SetRapleyScore(0);
            SetPacmomScore(0);
            SetPacmomLives(3);

            pacmom.gameController = this;

            foreach (Transform coin in coins)
            {
                coin.gameObject.SetActive(true);
                coin.GetComponent<Coin>().gameController = this;
            }

            foreach (Transform vacuum in vacuums)
            {
                vacuum.gameObject.SetActive(true);
                vacuum.GetComponent<Vacuum>().gameController = this;
            }

            rapley.ResetState();
            rapleySprite.GetNormalSprite();

            pacmom.ResetState();
            pacmomSprite.GetNormalSprite();

            for (int i=0; i<dustSprite.Length; i++)
                dustSprite[i].GetNormalSprite();
        }

        private void SetRapleyScore(int score)
        {
            rapleyScore = score;
        }

        private void SetPacmomScore(int score)
        {
            pacmomScore = score;
        }

        private void SetPacmomLives(int lives)
        {
            pacmomLives = lives;
        }

        public void UseVacuum(Vacuum vacuum)
        {
            vacuum.gameObject.SetActive(false);

            StopAllCoroutines();
            StartCoroutine("VacuumTime");
        }

        private IEnumerator VacuumTime()
        {
            pacmom.VacuumMode(true);
            rapleySprite.GetFrightendSprite();
            for (int i = 0; i < dustSprite.Length; i++)
                dustSprite[i].GetFrightendSprite();
            Invoke("VaccumBlink", vacuumDuration - vacuumEndDuration);

            yield return new WaitForSeconds(vacuumDuration);

            pacmom.VacuumMode(false);
            rapleySprite.GetNormalSprite();
            for (int i = 0; i < dustSprite.Length; i++)
                dustSprite[i].GetNormalSprite();
        }

        private void VaccumBlink()
        {
            pacmom.VacuumModeAlmostOver();
        }

        public void CoinEaten(Coin coin, string who)
        {
            coin.gameObject.SetActive(false);

            if (who == GlobalConst.PlayerStr)
                SetRapleyScore(rapleyScore + 1);
            else if (who == GlobalConst.PacmomStr)
                SetPacmomScore(pacmomScore + 1);

            if (!HasRemainingCoins())
            {
                GameOver();
            }
        }

        public void RapleyEaten()
        {
            Debug.Log("라플리 먹힘");

            TakeHalfCoins(false);
            rapley.transform.position = new Vector3(0, 0, rapley.transform.position.z);
        }

        public void PacmomEaten()
        {
            // ToDO: 유령한테 죽을 때 처리

            SetPacmomLives(pacmomLives - 1);
            TakeHalfCoins(true);

            if (pacmomLives > 0)
            {
                Debug.Log("팩맘 목수뮤 -1");
                ResetStates();
            }
            else
            {
                Debug.Log("팩맘 죽음");
                GameOver();
            }
        }

        private void TakeHalfCoins(bool isRapleyTake)
        {
            if (isRapleyTake)
            {
                int score = pacmomScore / 2;
                SetRapleyScore(rapleyScore + score);
                SetPacmomScore(pacmomScore - score);
            }
            else
            {
                int score = rapleyScore / 2;
                SetPacmomScore(pacmomScore + score);
                SetRapleyScore(rapleyScore - score);
            }
        }

        private void ResetStates()
        {
            rapley.movement.ResetState();
            pacmom.movement.ResetState();
        }

        private void GameOver()
        {
            Debug.Log("Game Over");

            rapley.movement.Stop();
            pacmom.movement.Stop();

            if (pacmomLives == 0)
                pacmom.PacmomDead();

            if (HasRemainingCoins())
            {
                Debug.Log("라플리 점수: " + rapleyScore);
                Debug.Log("팩맘 점수: " + pacmomScore);

                StartCoroutine(GetRemaningCoins());
            }
            else
            {
                Debug.Log("최종 라플리 점수: " + rapleyScore);
                Debug.Log("최종 팩맘 점수: " + pacmomScore);
            }
        }

        private IEnumerator GetRemaningCoins()
        {
            Debug.Log("최종 점수 계산 중");

            foreach (Transform coin in coins)
            {
                if (coin.gameObject.activeSelf)
                {
                    SetRapleyScore(rapleyScore + 1);
                    coin.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.05f);
                }
            }

            Debug.Log("최종 라플리 점수: " + rapleyScore);
            Debug.Log("최종 팩맘 점수: " + pacmomScore);
        }

        private bool HasRemainingCoins()
        {
            foreach (Transform coin in coins)
            {
                if (coin.gameObject.activeSelf)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
