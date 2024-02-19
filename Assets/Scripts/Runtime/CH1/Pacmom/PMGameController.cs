using Runtime.Common.View;
using Runtime.Data.Original;
using Runtime.ETC;
using Runtime.InGameSystem;
using Runtime.Interface;
using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMGameController : MonoBehaviour
    {
        #region 선언
        [Header("=Contoller=")]
        private PMSprite spriteController;
        [SerializeField]
        private PMData dataController;
        public SoundSystem soundSystem;
        [SerializeField]
        private Timer timer;
        [SerializeField]
        private InGameDialogue dialogue;

        [Header("=Character=")]
        [SerializeField]
        private Rapley rapley;

        [SerializeField]
        private Pacmom pacmom;
        private AI pacmomAI;

        [SerializeField]
        private Dust[] dusts = new Dust[GlobalConst.DustCnt];
        private AI[] dustAIs = new AI[GlobalConst.DustCnt];
        private DustRoom[] dustRooms = new DustRoom[GlobalConst.DustCnt];

        [Header("=Else=")]
        [SerializeField]
        private GameObject Door;
        [SerializeField]
        private int inRoom = 2;
        [field: SerializeField]
        public bool isGameOver { get; private set; } = false;
        private readonly float vacuumDuration = 10f;
        private readonly float vacuumEndDuration = 3f;
        private bool isMoving;

        private IProvider<ControlsData> ControlsDataProvider => DataProviderManager.Instance.ControlsDataProvider;
        private GameOverControls GameOverControls => ControlsDataProvider.Get().GameOverControls;

        [SerializeField]
        private SettingsUIView settingsUIView;
        #endregion

        #region Awake
        private void Awake()
        {
            AssignComponent();
            AssignController();
            SetSettingUI();
        }

        private void SetSettingUI()
        {
            GameOverControls.UI.Enable();
            GameOverControls.UI.GameSetting.performed += _ =>
            {
                settingsUIView.GameSettingToggle();
                Time.timeScale = (Time.timeScale == 0 ? 1 : 0);
            };
        }

        private void AssignComponent()
        {
            spriteController = GetComponent<PMSprite>();
            dataController = GetComponent<PMData>();

            pacmomAI = pacmom.GetComponent<AI>();
            for (int i = 0; i < dusts.Length; i++)
            {
                dustAIs[i] = dusts[i].GetComponent<AI>();
                dustRooms[i] = dusts[i].GetComponent<DustRoom>();
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
            dataController.InitData();

            ResetStates();
            SetCharacterMove(true);

            timer.SetTimer(true);
        }
        #endregion

        #region End
        public void GameOver()
        {
            soundSystem.StopMusic();
            timer.SetTimer(false);
            isGameOver = true;

            SetCharacterMove(false);

            if (dataController.HasRemainingCoins())
                dialogue.GameOverDialogue();

            StartCoroutine(dataController.GetRemaningCoins());
        }
        #endregion

        #region Vacuum Mode
        public void UseVacuum()
        {
            bool WasVacuumMode = pacmom.ai.isStronger;

            dialogue.VacuumDialogue(WasVacuumMode);

            if (!WasVacuumMode)
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
            spriteController.SetVacuumModeSprites();
            SetVacuumSpeed();
            SetVacuumMode(true);
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

            isMoving = move;
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
                    dustRooms[i].ExitRoom(GlobalConst.DustCnt - inRoom);
                    inRoom--;
                }
            }
        }
        #endregion

        #region Eaten
        public void RapleyEaten()
        {
            soundSystem.PlayEffect("PacmomEat");

            dataController.TakeHalfCoins(false);
            rapley.ResetState();
        }

        public void DustEaten(Dust dust)
        {
            soundSystem.PlayEffect("PacmomEat");

            dust.movement.SetCanMove(false);
            dust.movement.ResetState();
            dust.GetComponent<DustRoom>().SetInRoom(true);
            inRoom++;

            dialogue.BeCaughtDialogue(dust.dustID);
        }

        public void PacmomEatenByRapley()
        {
            PacmomEaten();

            dataController.TakeHalfCoins(true);
            LoseLife();
        }

        public void PacmomEatenByDust(int ID = 0)
        {
            PacmomEaten();

            dialogue.CatchDialogue(ID);
            StartCoroutine(dataController.ReleaseHalfCoins());

            SetCharacterMove(false);
        }

        public void AfterPacmomEatenByDust()
        {
            dialogue.HideBubble();
            SetCharacterMove(true);
            LoseLife();
        }

        private void PacmomEaten()
        {
            soundSystem.PlayEffect("PacmomStun");
            Debug.Log("팩맘 먹힘");
        }

        public void LoseLife()
        {
            dataController.LosePacmomLife();

            if (dataController.IsPacmomAlive())
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

        public Vector3 GetPacmomPos()
        {
            return pacmom.movement.rigid.position;
        }

        public void CoinEatenByRapley()
        {
            if (!isMoving)
                return;

            soundSystem.PlayEffect("RapleyEatCoin");

            dataController.RapleyScore1Up();

            if (!dataController.HasRemainingCoins())
            {
                GameOver();
            }
        }

        public void CoinEatenByPacmom()
        {
            if (!isMoving)
                return;

            soundSystem.PlayEffect("PacmomEatCoin");

            dataController.PacmomScore1Up();

            if (!dataController.HasRemainingCoins())
            {
                GameOver();
            }
        }
        #endregion
    }
}