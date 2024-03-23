using TMPro;
using UnityEngine;
using Sound = Runtime.ETC.Sound;

namespace Runtime.CH1.Pacmom
{
    public class Timer : MonoBehaviour
    {
        public PMGameController GameController;
        private TextMeshProUGUI _timerTxt;
        [SerializeField]
        private float _timelimit = 180;
        private int _min;
        private int _sec;
        public bool IsTimerRunning { get; private set; }
        private bool _isAlmostOver = false;

        private void Awake()
        {
            _timerTxt = GetComponent<TextMeshProUGUI>();
        }

        public void SetTimer(bool isTimerRunning)
        {
            IsTimerRunning = isTimerRunning;
        }

        void Update()
        {
            if (!IsTimerRunning)
                return;
            
            CountTime();
            ShowTimer();
        }

        private void CountTime()
        {
            _timelimit -= Time.deltaTime;
            _min = (int)_timelimit / 60;
            _sec = ((int)_timelimit - _min * 60) % 60;

            AlmostOver();
            Over();
        }

        private void ShowTimer()
        {
            string minStr = (_min < 10 ? "0" : "") + _min.ToString();
            string secStr = (_sec < 10 ? "0" : "") + _sec.ToString();
            _timerTxt.text = minStr + ":" + secStr;
        }

        private void Over()
        {
            if (_timelimit < 1f)
            {
                GameController?.GameOver();
                if (GameController is null)
                    SetTimer(false);
            }
        }

        private void AlmostOver()
        {
            if (!_isAlmostOver && _timelimit <= 10f)
            {
                _isAlmostOver = true;
                Invoke("TicToc", 1f);
            }
        }

        private void TicToc()
        {
            if (_isAlmostOver && IsTimerRunning)
            {
                Managers.Sound.Play(Sound.Effect, "Pacmom_SFX_08");
                Invoke("TicToc", 1f);
            }
        }
    }
}