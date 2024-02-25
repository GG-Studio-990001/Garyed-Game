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
        [SerializeField]
        private Dust[] dusts = new Dust[GlobalConst.DustCnt];
        private DustRoom[] dustRooms = new DustRoom[GlobalConst.DustCnt];

        [Header("=Else=")]
        [SerializeField]
        private GameObject Door;
        [field: SerializeField]
        public bool isGameOver { get; private set; } = false;
        private readonly float vacuumDuration = 10f;
        private readonly float vacuumEndDuration = 3f;
        private bool isMoving;

        #region Setting UI
        private IProvider<ControlsData> ControlsDataProvider => DataProviderManager.Instance.ControlsDataProvider;
        private GameOverControls GameOverControls => ControlsDataProvider.Get().GameOverControls;

        [SerializeField]
        private SettingsUIView settingsUIView;
        #endregion

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

            for (int i = 0; i < dusts.Length; i++)
            {
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
            SetCharacterMove(false);
        }

        public void StartGame()
        {
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
            pacmom.SetAIStronger(isVacuumMode);
            for (int i = 0; i < dusts.Length; i++)
            {
                dusts[i].SetAIStronger(!isVacuumMode);
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
                    int cntInRoom = GlobalConst.DustCnt - ((dustRooms[0].isInRoom ? 1 : 0) + (dustRooms[1].isInRoom ? 1 : 0));
                    dustRooms[i].ExitRoom(cntInRoom);
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

            dialogue.BeCaughtDialogue(dust.dustID);
        }

        public void PacmomEatenByRapley()
        {
            soundSystem.PlayEffect("PacmomStun");

            dataController.TakeHalfCoins(true);
            LoseLife();
        }

        public void PacmomEatenByDust(int ID)
        {
            soundSystem.PlayEffect("PacmomStun");

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