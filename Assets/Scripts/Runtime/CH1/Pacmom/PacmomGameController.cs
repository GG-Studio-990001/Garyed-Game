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
        private SpriteController rapleySprite;
        [SerializeField]
        private Pacmom pacmom;
        private PacmomSpriteController pacmomSprite;
        [SerializeField]
        private Dust[] dusts;
        [SerializeField]
        private DustSpriteController[] dustSprites;
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
            rapleySprite = rapley.gameObject.GetComponent<SpriteController>();
            pacmomSprite = pacmom.gameObject.GetComponent<PacmomSpriteController>();
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

            for (int i=0; i<dustSprites.Length; i++)
            {
                dusts[i].ResetState();
                dustSprites[i].GetNormalSprite();
            }
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
            pacmom.movement.speedMultiplier = 1.2f;

            rapleySprite.GetVacuumModeSprite();
            rapley.movement.speedMultiplier = 0.7f;

            for (int i = 0; i < dustSprites.Length; i++)
            {
                dustSprites[i].GetVacuumModeSprite();
                dusts[i].movement.speedMultiplier = 0.7f;
            }

            yield return new WaitForSeconds(vacuumDuration - vacuumEndDuration);

            pacmom.VacuumModeAlmostOver();

            yield return new WaitForSeconds(vacuumEndDuration);

            pacmom.VacuumMode(false);
            pacmom.movement.speedMultiplier = 1f;

            rapleySprite.GetNormalSprite();
            rapley.movement.speedMultiplier = 1f;

            for (int i = 0; i < dustSprites.Length; i++)
            {
                dustSprites[i].GetNormalSprite();
                dusts[i].movement.speedMultiplier = 1f;
            }

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
