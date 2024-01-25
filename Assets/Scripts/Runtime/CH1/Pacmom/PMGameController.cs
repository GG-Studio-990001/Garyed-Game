using Runtime.ETC;
using Runtime.InGameSystem;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMGameController : MonoBehaviour
    {
        #region 선언
        private PMSpriteController spriteController;
        [SerializeField]
        private PMUIController uiController;
        public SoundSystem soundSystem;
        [SerializeField]
        private Timer timer;
        [SerializeField]
        private InGameDialogue dialogue;
        [SerializeField]
        private PMEnding ending;

        [Header("=Character=")]
        [SerializeField]
        private Rapley rapley;

        [SerializeField]
        private Pacmom pacmom;
        private AI pacmomAI;

        [SerializeField]
        private Dust[] dusts = new Dust[GlobalConst.DustCnt];
        private AI[] dustAIs = new AI[GlobalConst.DustCnt];
        private Room[] dustRooms = new Room[GlobalConst.DustCnt];

        [Header("=Else=")]
        [SerializeField]
        private Transform coins;
        [SerializeField]
        private Transform vacuums;
        [SerializeField]
        private GameObject Door;

        [Header("=Variable=")]
        [SerializeField]
        private int inRoom = 2;
        [SerializeField]
        private bool isGameOver = false;
        private int rapleyScore;
        private int pacmomScore;
        private int pacmomLives;
        private readonly float vacuumDuration = 10f;
        private readonly float vacuumEndDuration = 3f;
        #endregion

        #region Awake
        private void Awake()
        {
            AssignComponent();
            AssignController();
        }

        private void AssignComponent()
        {
            spriteController = GetComponent<PMSpriteController>();

            pacmomAI = pacmom.GetComponent<AI>();

            for (int i = 0; i < dusts.Length; i++)
            {
                dustAIs[i] = dusts[i].GetComponent<AI>();
                dustRooms[i] = dusts[i].GetComponent<Room>();
            }
        }

        private void AssignController()
        {
            timer.gameController = this;
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
        #endregion

        #region Set
        private void SetRapleyScore(int score)
        {
            rapleyScore = score;
            uiController.ShowRapleyScore(score);
        }

        private void SetPacmomScore(int score)
        {
            pacmomScore = score;
            uiController.ShowPacmomScore(score);
        }

        private void SetPacmomLives(int lives)
        {
            if (lives < 0)
                return;

            pacmomLives = lives;
        }
        #endregion

        #region Start
        private void Start()
        {
            spriteController.SetNormalSprites();
            SetCharacterMove(false);
        }

        public void StartGame()
        {
            SetRapleyScore(0);
            SetPacmomScore(0);
            SetPacmomLives(3);

            ResetStates();
            SetCharacterMove(true);

            timer.SetTimer(true);
        }
        #endregion

        #region End
        public void GameOver()
        {
            timer.SetTimer(false);
            isGameOver = true;

            SetCharacterMove(false);
            dialogue.GameOverDialogue();

            if (HasRemainingCoins())
            {
                StartCoroutine(GetRemaningCoins());
            }
            else
            {
                ChooseAWinner();
            }
        }

        private void ChooseAWinner()
        {
            soundSystem.StopMusic();
            soundSystem.StopSFX();

            if (rapleyScore > pacmomScore)
            {
                ending.RapleyWin();
            }
            else
            {
                ending.PacmomWin();
            }
        }
        #endregion

        #region Vacuum Mode
        public void UseVacuum()
        {
            if (!pacmom.ai.isStronger)
            {
                soundSystem.PlayMusic("StartVacuum");
            }
            else
            {
                StopCoroutine("VacuumTime");
                soundSystem.StopMusic();

                soundSystem.PlayMusic("ContinueVacuum");
            }

            StartCoroutine("VacuumTime");
        }

        private IEnumerator VacuumTime()
        {
            VacuumModeOn();

            yield return new WaitForSeconds(vacuumDuration - vacuumEndDuration);
            if (isGameOver) yield break;
            
            spriteController.SetPacmomBlinkSprite();

            yield return new WaitForSeconds(vacuumEndDuration);
            if (isGameOver) yield break;

            VacuumModeOff();
        }

        private void VacuumModeOn()
        {
            spriteController.SetVaccumModeSprites();
            SetVacuumSpeed();
            SetVacuumMode(true);
            dialogue.VacuumDialogue();
        }

        private void VacuumModeOff()
        {
            spriteController.SetNormalSprites();
            SetNormalSpeed();
            SetVacuumMode(false);

            DustExitRoom();
        }
        #endregion

        #region Common
        private void ResetStates()
        {
            spriteController.SetNormalSprites();

            rapley.ResetState();
            pacmom.ResetState();

            for (int i = 0; i < dusts.Length; i++)
            {
                dusts[i].ResetState();
                dustRooms[i].SetInRoom(true);
            }
            inRoom = 2;

            DustExitRoom();
        }

        private void SetCharacterMove(bool move)
        {
            rapley.movement.SetCanMove(move);
            pacmom.movement.SetCanMove(move);
            for (int i = 0; i < dusts.Length; i++)
                dusts[i].movement.SetCanMove(move);
        }

        private void SetVacuumMode(bool isVacuumMode)
        {
            pacmom.VacuumMode(isVacuumMode);
            pacmomAI.SetStronger(isVacuumMode);
            for (int i = 0; i < dusts.Length; i++)
            {
                dustAIs[i].SetStronger(!isVacuumMode);
                dusts[i].movement.SetEyeNormal(!isVacuumMode);
            }

            Door.SetActive(isVacuumMode);
        }

        private void SetVacuumSpeed()
        {
            pacmom.movement.SetSpeedMultiplier(1.2f);
            rapley.movement.SetSpeedMultiplier(0.7f);
            for (int i = 0; i < dusts.Length; i++)
                dusts[i].movement.SetSpeedMultiplier(0.7f);
        }

        private void SetNormalSpeed()
        {
            pacmom.movement.SetSpeedMultiplier(1f);
            rapley.movement.SetSpeedMultiplier(1f);
            for (int i = 0; i < dusts.Length; i++)
                dusts[i].movement.SetSpeedMultiplier(1f);
        }

        private void DustExitRoom()
        {
            for (int i = 0; i < dusts.Length; i++)
            {
                if (dustRooms[i].isInRoom)
                {
                    dustRooms[i].ExitRoom((GlobalConst.DustCnt - inRoom + 1) * 5);
                    inRoom--;
                }
            }
        }
        #endregion

        #region Eaten
        public void RapleyEaten()
        {
            soundSystem.PlayEffect("PacmomEat");

            TakeHalfCoins(false);
            rapley.ResetState();
        }

        public void DustEaten(Dust dust)
        {
            soundSystem.PlayEffect("PacmomEat");

            dust.movement.SetCanMove(false);
            dust.ResetState();
            dust.GetComponent<Room>().SetInRoom(true);
            inRoom++;
        }

        public void PacmomEaten(string byWhom)
        {
            soundSystem.PlayEffect("PacmomStun");

            Debug.Log("팩맘 먹힘");

            if (byWhom == GlobalConst.PlayerStr)
                TakeHalfCoins(true);
            else if (byWhom == GlobalConst.DustStr)
                ReleaseHalfCoins();

            SetPacmomLives(pacmomLives - 1);
            uiController.LosePacmomLife(pacmomLives);

            if (pacmomLives > 0)
            {
                ResetStates();
            }
            else
            {
                pacmom.SetRotateToZero();
                spriteController.SetPacmomDieSprite();

                GameOver();
            }
        }

        public void CoinEaten(string byWhom)
        {
            if (byWhom == GlobalConst.PlayerStr)
            {
                soundSystem.PlayEffect("RapleyEatCoin");

                SetRapleyScore(rapleyScore + 1);
            }
            else if (byWhom == GlobalConst.PacmomStr)
            {
                soundSystem.PlayEffect("PacmomEatCoin");

                SetPacmomScore(pacmomScore + 1);
            }

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
            soundSystem.PlayEffect("DropCoin");

            int score = pacmomScore / 2;
            SetPacmomScore(pacmomScore - score);
            Debug.Log("팩맘 코인 " + score + "개 떨굼");

            foreach (Transform coin in coins)
            {
                if (score <= 0)
                    break;

                if (!coin.gameObject.activeSelf)
                {
                    coin.gameObject.SetActive(true);
                    score--;
                }
            }
        }

        private IEnumerator GetRemaningCoins()
        {
            foreach (Transform coin in coins)
            {
                if (coin.gameObject.activeSelf)
                {
                    soundSystem.PlayEffect("RapleyEatCoin");

                    SetRapleyScore(rapleyScore + 1);
                    coin.gameObject.SetActive(false);
                    yield return new WaitForSeconds(0.03f);
                }
            }
            ChooseAWinner();
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