using DG.Tweening.Core.Easing;
using Runtime.ETC;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PacmomGameController : MonoBehaviour
    {
        #region 선언
        [Header("Rapley")]
        [SerializeField]
        private Rapley rapley;
        private SpriteController rapleySprite;

        [Header("Pacmom")]
        [SerializeField]
        private Pacmom pacmom;
        private PacmomSpriteController pacmomSprite;
        private AI pacmomAI;

        [Header("Dusts")]
        [SerializeField]
        private Dust[] dusts;
        private DustSpriteController[] dustSprites = new DustSpriteController[GlobalConst.DustCnt];
        private AI[] dustAIs = new AI[GlobalConst.DustCnt];
        private Room[] dustRooms = new Room[GlobalConst.DustCnt];

        [Header("Else")]
        [SerializeField]
        private Transform coins;
        [SerializeField]
        private Transform vacuums;
        [SerializeField]
        private GameObject Door;
        [SerializeField]
        private int inRoom = 2;
        private readonly float vacuumDuration = 10f;
        private readonly float vacuumEndDuration = 3f;
        
        public int rapleyScore { get; private set; }
        public int pacmomScore { get; private set; }
        public int pacmomLives { get; private set; }
        #endregion

        #region Set
        private void Awake()
        {
            rapleySprite = rapley.gameObject.GetComponent<SpriteController>();

            pacmomSprite = pacmom.gameObject.GetComponent<PacmomSpriteController>();
            pacmomAI = pacmom.gameObject.GetComponent<AI>();

            for (int i = 0; i < dusts.Length; i++)
            {
                dustSprites[i] = dusts[i].gameObject.GetComponentInChildren<DustSpriteController>();
                dustAIs[i] = dusts[i].gameObject.GetComponent<AI>();
                dustRooms[i] = dusts[i].gameObject.GetComponent<Room>();
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
        #endregion

        #region Start & End
        private void Start()
        {
            StartGame();
        }

        private void StartGame()
        {
            SetRapleyScore(0);
            SetPacmomScore(0);
            SetPacmomLives(3);

            AssignController();

            ResetStates();
        }

        private void AssignController()
        {
            pacmom.gameController = this;

            for (int i = 0; i < dusts.Length; i++)
            {
                dusts[i].gameController = this;
            }

            foreach (Transform coin in coins)
            {
                coin.GetComponent<Coin>().gameController = this;
            }

            foreach (Transform vacuum in vacuums)
            {
                vacuum.GetComponent<Vacuum>().gameController = this;
            }
        }

        private void ResetStates()
        {
            rapley.ResetState();
            rapleySprite.GetNormalSprite();

            pacmom.ResetState();
            pacmomSprite.GetNormalSprite();

            for (int i = 0; i < dusts.Length; i++)
            {
                dusts[i].ResetState();
                dustSprites[i].GetNormalSprite();
                dustRooms[i].ExitRoom(GlobalConst.DustCnt - inRoom);
                inRoom--;
            }
        }

        private void GameOver()
        {
            Debug.Log("Game Over");

            rapley.movement.Stop();
            pacmom.movement.Stop();
            for (int i = 0; i < dusts.Length; i++)
            {
                dusts[i].movement.Stop();
            }

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
        #endregion

        #region Vacuum Mode
        public void UseVacuum(Vacuum vacuum)
        {
            vacuum.gameObject.SetActive(false);

            StopAllCoroutines();
            StartCoroutine("VacuumTime");
        }

        private IEnumerator VacuumTime()
        {
            VacuumModeOn();

            yield return new WaitForSeconds(vacuumDuration - vacuumEndDuration);

            pacmom.VacuumModeAlmostOver();

            yield return new WaitForSeconds(vacuumEndDuration);

            VacuumModeOff();
        }

        private void VacuumModeOn()
        {
            pacmom.VacuumMode(true);
            pacmom.movement.speedMultiplier = 1.2f;
            pacmomAI.isStronger = true;

            rapleySprite.GetVacuumModeSprite();
            rapley.movement.speedMultiplier = 0.7f;

            for (int i = 0; i < dustSprites.Length; i++)
            {
                dustSprites[i].GetVacuumModeSprite();
                dusts[i].movement.speedMultiplier = 0.7f;
                dustAIs[i].isStronger = false;
            }
            Door.SetActive(true);
        }

        private void VacuumModeOff()
        {
            pacmom.VacuumMode(false);
            pacmom.movement.speedMultiplier = 1f;
            pacmomAI.isStronger = false;

            rapleySprite.GetNormalSprite();
            rapley.movement.speedMultiplier = 1f;

            for (int i = 0; i < dustSprites.Length; i++)
            {
                dustSprites[i].GetNormalSprite();
                dusts[i].movement.speedMultiplier = 1f;
                dustAIs[i].isStronger = true;
            }

            Door.SetActive(false);
            for (int i = 0; i < dustSprites.Length; i++)
            {
                if (dustRooms[i].isInRoom)
                {
                    dustRooms[i].ExitRoom(GlobalConst.DustCnt - inRoom);
                    inRoom--;
                }
            }
        }
        #endregion

        #region Eaten
        public void RapleyEaten()
        {
            Debug.Log("라플리 먹힘");

            TakeHalfCoins(false);
            rapley.movement.ResetState();
            inRoom++;
        }

        public void PacmomDustCollision(Dust dust)
        {
            if (pacmom.isVacuumMode)
            {
                DustEaten(dust);
            }
            else
            {
                PacmomEaten(GlobalConst.DustStr);
            }
        }

        public void DustEaten(Dust dust)
        {
            Debug.Log("먼지유령 먹힘");
            dust.movement.ResetState();
            dust.gameObject.GetComponent<Room>().isInRoom = true;
            inRoom++;
        }

        public void PacmomEaten(string byWhom)
        {
            if (byWhom == GlobalConst.DustStr)
            {
                ReleaseHalfCoins();
            }
            else if (byWhom == GlobalConst.PlayerStr)
            {
                TakeHalfCoins(true);
            }

            SetPacmomLives(pacmomLives - 1);

            if (pacmomLives > 0)
            {
                ResetStates();
            }
            else
            {
                GameOver();
            }
        }

        public void CoinEaten(Coin coin, string byWhom)
        {
            if (byWhom == GlobalConst.PlayerStr)
                SetRapleyScore(rapleyScore + 1);
            else if (byWhom == GlobalConst.PacmomStr)
                SetPacmomScore(pacmomScore + 1);

            if (!HasRemainingCoins())
            {
                GameOver();
            }
        }
        #endregion

        #region About Coin
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

        private void ReleaseHalfCoins()
        {
            int score = pacmomScore / 2;
            SetPacmomScore(pacmomScore - score);
            Debug.Log("팩맘 코인 " + score + "개 떨굼");

            foreach (Transform coin in coins)
            {
                if (score != 0 && !coin.gameObject.activeSelf)
                {
                    coin.gameObject.SetActive(true);
                    score--;
                }
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
                    yield return new WaitForSeconds(0.03f);
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
        #endregion
    }
}
