using Runtime.InGameSystem;
using System.Collections;
using TMPro;
using UnityEngine;
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH1.Pacmom
{
    public class PMEnding : MonoBehaviour
    {
        private PMShader _shader;
        [SerializeField]
        private ControlSystem _controlSystem;
        [SerializeField]
        private SceneSystem _sceneSystem;
        [SerializeField]
        private GameObject _timeline3;
        [SerializeField]
        private SpriteControl[] _spriteControls = new SpriteControl[6];
        [SerializeField]
        private TextMeshProUGUI _resultCoinTxt;

        public void Awake()
        {
            _shader = GetComponent<PMShader>();
        }

        public void RapleyWin()
        {
            Managers.Data.IsPacmomCleared = true;
            GamePlayed();

            Debug.Log("라플리 승리");
            _timeline3.SetActive(true);
        }

        public void PacmomWin()
        {
            GamePlayed();

            Debug.Log("팩맘 승리");
            StartCoroutine(nameof(PacmomGameOver));
        }

        private void GamePlayed()
        {
            if (!Managers.Data.IsPacmomPlayed)
                Managers.Data.IsPacmomPlayed = true;

            Managers.Data.SaveGame();
        }

        public void GetAllNormalSprite()
        {
            for (int i = 0; i < _spriteControls.Length; i++)
            {
                _spriteControls[i].GetNormalSprite();
            }
        }

        public void GetAllVacuumModeSprite()
        {
            for (int i = 0; i < _spriteControls.Length; i++)
            {
                _spriteControls[i].GetVacuumModeSprite();
            }
        }

        IEnumerator PacmomGameOver()
        {
            Time.timeScale = 0;
            _shader.ChangeBleedAmount();

            yield return new WaitForSecondsRealtime(3f);

            Time.timeScale = 1f;

            ExitPacmom();
        }

        public void ExitPacmom()
        {
            _controlSystem.ExitNewControl();
            _sceneSystem.LoadScene("CH1");
        }

        public void ShowResultCoin(int finalScore)
        {
            StartCoroutine(nameof(ResultCoinUp), finalScore);
        }

        private IEnumerator ResultCoinUp(int finalScore)
        {
            string scoreStr = _resultCoinTxt.text[1..];
            int score = int.Parse(scoreStr);
            float changeTime = 0.01f;

            while (score < finalScore)
            {
                Managers.Sound.Play(Sound.Effect, "Pacmom_SFX_10");
                score++;
                _resultCoinTxt.text = "x" + score.ToString();
                yield return new WaitForSeconds(changeTime);
            }
        }
    }
}