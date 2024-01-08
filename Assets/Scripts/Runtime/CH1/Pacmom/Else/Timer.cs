using TMPro;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Timer : MonoBehaviour
    {
        public PMGameController gameController;
        [SerializeField]
        private TextMeshProUGUI timerTxt;
        [SerializeField]
        private float timelimit;
        private int min;
        private int sec;
        public bool isTimerRunning { get; private set; }
        private bool isAlmostOver = false;

        public void SetTimer(bool isTimerRunning)
        {
            this.isTimerRunning = isTimerRunning;
        }

        void Update()
        {
            if (!isTimerRunning)
                return;
            
            CountTime();
            ShowTimer();
        }

        private void CountTime()
        {
            timelimit -= Time.deltaTime;
            min = (int)timelimit / 60;
            sec = ((int)timelimit - min * 60) % 60;

            AlmostOver();
            Over();
        }

        private void ShowTimer()
        {
            string minStr = (min < 10 ? "0" : "") + min.ToString();
            string secStr = (sec < 10 ? "0" : "") + sec.ToString();
            timerTxt.text = minStr + ":" + secStr;
        }

        private void Over()
        {
            if (timelimit < 1f)
            {
                gameController?.GameOver();
                if (gameController == null)
                    SetTimer(false);
            }
        }

        private void AlmostOver()
        {
            if (!isAlmostOver && timelimit <= 10f)
            {
                isAlmostOver = true;
                Invoke("TicToc", 1f);
            }
        }

        private void TicToc()
        {
            if (isAlmostOver && isTimerRunning)
            {
                gameController.soundSystem.PlayEffect("TicToc");
                Invoke("TicToc", 1f);
            }
            
        }
    }
}