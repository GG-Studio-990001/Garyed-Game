using TMPro;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class Timer : MonoBehaviour
    {
        [SerializeField]
        private PMGameController gameController;
        [SerializeField]
        private TextMeshProUGUI timerTxt;
        [SerializeField]
        private float timelimit;
        private int min;
        private int sec;
        public bool isTimerRunning { get; private set; }

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

            if (timelimit < 1f)
                gameController?.GameOver();
        }

        private void ShowTimer()
        {
            string minStr = (min < 10 ? "0" : "") + min.ToString();
            string secStr = (sec < 10 ? "0" : "") + sec.ToString();
            timerTxt.text = minStr + ":" + secStr;
        }

        public void StartTimer()
        {
            isTimerRunning = true;
        }

        public void StopTimer()
        {
            isTimerRunning = false;
        }
    }
}