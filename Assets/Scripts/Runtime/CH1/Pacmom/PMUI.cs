using System.Collections;
using TMPro;
using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class PMUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] pacmomLives = new GameObject[3];
        [SerializeField]
        private TextMeshProUGUI pacmomScoreTxt;
        [SerializeField]
        private TextMeshProUGUI rapleyScoreTxt;

        public void LosePacmomLife(int nowLives)
        {
            pacmomLives[nowLives].SetActive(false);
        }

        public void ShowPacmomScore(int newScore)
        {
            StartCoroutine("ChangePacmomScore", newScore);
        }

        public void ShowRapleyScore(int newScore)
        {
            StartCoroutine("ChangeRapleyScore", newScore);
        }

        private IEnumerator ChangePacmomScore(int newScore)
        {
            string scoreStr = pacmomScoreTxt.text.Substring(1);
            int score = int.Parse(scoreStr);

            while (score != newScore)
            {
                score += (score < newScore ? 1 : -1);
                pacmomScoreTxt.text = "x" + score.ToString();
                yield return new WaitForSeconds(0.03f); // 0.03 코인 드롭/획득 시간 => const로 지정? // 리터럴 값 지양
            }
        }

        private IEnumerator ChangeRapleyScore(int newScore)
        {
            string scoreStr = rapleyScoreTxt.text.Substring(1);
            int score = int.Parse(scoreStr);

            while (score != newScore)
            {
                score += (score < newScore ? 1 : -1);
                rapleyScoreTxt.text = "x" + score.ToString();
                yield return new WaitForSeconds(0.03f);
            }
        }
    }
}