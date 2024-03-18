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
        private PMSprite _spriteController;
        private PMData _dataController;
        [Header("=Contoller=")]
        public SoundSystem soundSystem;
        [SerializeField]
        private InGameDialogue _dialogue;
        [SerializeField]
        private Timer _timer;
        [SerializeField]
        private Door _door;

        [Header("=Character=")]
        [SerializeField]
        private Rapley _rapley;
        [SerializeField]
        private Pacmom _pacmom;
        [SerializeField]
        private Dust[] _dusts = new Dust[GlobalConst.DustCnt];
        private DustRoom[] _dustRooms = new DustRoom[GlobalConst.DustCnt];

        public bool isGameOver { get; private set; } = false;
        private readonly float _vacuumDuration = 10f;
        private readonly float _vacuumEndDuration = 3f;
        private bool _isMoving;
        private bool _isVacuumMode = false;

        private IProvider<ControlsData> _controlsDataProvider => DataProviderManager.Instance.ControlsDataProvider;
        private GameOverControls _gameOverControls => _controlsDataProvider.Get().GameOverControls;

        [Header("=Setting UI=")]
        [SerializeField]
        private SettingsUIView _settingsUIView;
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
            _gameOverControls.UI.Enable();
            _gameOverControls.UI.GameSetting.performed += _ =>
            {
                _settingsUIView.GameSettingToggle();
                Time.timeScale = (Time.timeScale == 0 ? 1 : 0);
            };
        }

        private void AssignComponent()
        {
            _spriteController = GetComponent<PMSprite>();
            _dataController = GetComponent<PMData>();

            for (int i = 0; i < _dusts.Length; i++)
            {
                _dustRooms[i] = _dusts[i].GetComponent<DustRoom>();
            }
        }

        private void AssignController()
        {
            _timer.gameController = this;
            _pacmom.gameController = this;

            for (int i = 0; i < _dusts.Length; i++)
            {
                _dusts[i].gameController = this;
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

            _timer.SetTimer(true);
        }
        #endregion

        #region End
        public void GameOver()
        {
            soundSystem.StopMusic();
            _timer.SetTimer(false);
            isGameOver = true;

            SetCharacterMove(false);

            if (_dataController.HasRemainingCoins())
                _dialogue.GameOverDialogue();

            StartCoroutine(_dataController.GetRemaningCoins());
        }
        #endregion

        #region Vacuum Mode
        public void UseVacuum()
        {
            _dialogue.VacuumDialogue(_isVacuumMode);

            if (!_isVacuumMode)
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

            yield return new WaitForSeconds(_vacuumDuration - _vacuumEndDuration);
            if (isGameOver) yield break;

            _spriteController.SetPacmomBlinkSprite();

            yield return new WaitForSeconds(_vacuumEndDuration);
            if (isGameOver) yield break;

            VacuumModeOff();
        }

        private void VacuumModeOn()
        {
            _spriteController.SetVacuumModeSprites();
            SetVacuumSpeed();
            SetVacuumMode(true);
        }

        private void VacuumModeOff()
        {
            _spriteController.SetNormalSprites();
            SetNormalSpeed();
            SetVacuumMode(false);

            DustExitRoom();
        }
        #endregion

        #region Common
        private void ResetStates()
        {
            _spriteController.SetNormalSprites();

            _rapley.ResetState();
            _pacmom.ResetState();

            for (int i = 0; i < _dusts.Length; i++)
            {
                _dusts[i].ResetState();
                _dustRooms[i].SetInRoom(true);
            }

            DustExitRoom();
        }

        private void SetCharacterMove(bool move)
        {
            _rapley.movement.SetCanMove(move);
            _pacmom.movement.SetCanMove(move);
            for (int i = 0; i < _dusts.Length; i++)
                _dusts[i].movement.SetCanMove(move);

            _isMoving = move;
        }

        private void SetVacuumMode(bool isVacuumMode)
        {
            this._isVacuumMode = isVacuumMode;
            _pacmom.VacuumMode(isVacuumMode);
            _pacmom.SetStronger(isVacuumMode);
            for (int i = 0; i < _dusts.Length; i++)
            {
                _dusts[i].SetStronger(!isVacuumMode);
                _dusts[i].movement.SetEyeNormal(!isVacuumMode);
            }

            _door.ActiveDoor(isVacuumMode);
        }

        private void SetVacuumSpeed()
        {
            _pacmom.movement.SetSpeedMultiplier(1.2f);
            _rapley.movement.SetSpeedMultiplier(0.7f);
            for (int i = 0; i < _dusts.Length; i++)
                _dusts[i].movement.SetSpeedMultiplier(0.7f);
        }

        private void SetNormalSpeed()
        {
            _pacmom.movement.SetSpeedMultiplier(1f);
            _rapley.movement.SetSpeedMultiplier(1f);
            for (int i = 0; i < _dusts.Length; i++)
                _dusts[i].movement.SetSpeedMultiplier(1f);
        }

        private void DustExitRoom()
        {
            for (int i = 0; i < _dusts.Length; i++)
            {
                if (_dustRooms[i].isInRoom)
                {
                    int cntInRoom = GlobalConst.DustCnt - ((_dustRooms[0].isInRoom ? 1 : 0) + (_dustRooms[1].isInRoom ? 1 : 0));
                    _dustRooms[i].ExitRoom(cntInRoom);
                }
            }
        }
        #endregion

        #region Eaten
        public void RapleyEaten()
        {
            soundSystem.PlayEffect("PacmomEat");

            _dataController.TakeHalfCoins(false);
            _rapley.ResetState();
        }

        public void DustEaten(Dust dust)
        {
            soundSystem.PlayEffect("PacmomEat");

            dust.movement.SetCanMove(false);
            dust.movement.ResetState();
            dust.GetComponent<DustRoom>().SetInRoom(true);

            _dialogue.BeCaughtDialogue(dust.dustID);
        }

        public void PacmomEatenByRapley()
        {
            soundSystem.PlayEffect("PacmomStun");

            _dataController.TakeHalfCoins(true);
            LoseLife();
        }

        public void PacmomEatenByDust(int ID)
        {
            soundSystem.PlayEffect("PacmomStun");

            _dialogue.CatchDialogue(ID);
            StartCoroutine(_dataController.ReleaseHalfCoins());

            SetCharacterMove(false);
        }

        public void AfterPacmomEatenByDust()
        {
            _dialogue.HideBubble();
            SetCharacterMove(true);
            LoseLife();
        }

        public void LoseLife()
        {
            _dataController.LosePacmomLife();

            if (_dataController.IsPacmomAlive())
            {
                ResetStates();
            }
            else
            {
                _pacmom.SetRotateToZero();
                _spriteController.SetPacmomDieSprite();

                GameOver();
            }
        }

        public Vector3 GetPacmomPos()
        {
            return _pacmom.movement.rigid.position;
        }

        public void CoinEatenByRapley()
        {
            if (!_isMoving)
                return;

            soundSystem.PlayEffect("RapleyEatCoin");

            _dataController.RapleyScore1Up();

            if (!_dataController.HasRemainingCoins())
            {
                GameOver();
            }
        }

        public void CoinEatenByPacmom()
        {
            if (!_isMoving)
                return;

            soundSystem.PlayEffect("PacmomEatCoin");

            _dataController.PacmomScore1Up();

            if (!_dataController.HasRemainingCoins())
            {
                GameOver();
            }
        }
        #endregion
    }
}