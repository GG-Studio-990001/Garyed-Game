using Runtime.CH1.Pacmom;
using System.Collections;
using UnityEngine;

public class PMData : MonoBehaviour
{
    #region 선언
    private PMGameController gameController;
    [Header("=Contoller=")]
    [SerializeField]
    private PMUI uiController;
    [SerializeField]
    private PMEnding ending;
    [Header("=Item=")]
    [SerializeField]
    private Transform coins;
    [SerializeField]
    private Transform vacuums;
    private int rapleyScore;
    private int pacmomScore;
    private int pacmomLives;
    #endregion

    #region Awake & Start
    private void Awake()
    {
        gameController = GetComponent<PMGameController>();

        foreach (Transform coin in coins)
        {
            coin.GetComponent<Coin>().gameController = this.gameController;
        }

        foreach (Transform vacuum in vacuums)
        {
            vacuum.GetComponent<Vacuum>().gameController = this.gameController;
        }
    }

    private void Start()
    {
        SetRapleyScore(0);
        SetPacmomScore(0);
        SetPacmomLives(3);
    }
    #endregion

    #region Score & Lives
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

    public void RapleyScore1Up()
    {
        SetRapleyScore(rapleyScore + 1);
    }

    public void PacmomScore1Up()
    {
        SetPacmomScore(pacmomScore + 1);
    }

    public void LosePacmomLife()
    {
        SetPacmomLives(pacmomLives - 1);
        uiController.LosePacmomLife(pacmomLives);
    }

    public bool IsPacmomAlive()
    {
        return (pacmomLives > 0);
    }
    #endregion

    #region About Coins
    public void TakeHalfCoins(bool isRapleyTake)
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

    public IEnumerator ReleaseHalfCoins()
    {
        // 방에서 나오는 먼지 예외처리
        StopAllCoroutines();

        int score = pacmomScore / 2;
        SetPacmomScore(pacmomScore - score);

        // 떨굴 코인이 많으면 떨구는 시간 단축
        float releaseTime = (score >= 70 ? 0.02f : 0.03f);

        while (score > 0)
        {
            int rand = Random.Range(0, coins.childCount);
            Transform childCoin = coins.transform.GetChild(rand);
            Coin coin = childCoin.GetComponent<Coin>();

            if (!coin.gameObject.activeSelf)
            {
                coin.ResetCoin();
                score--;
                yield return new WaitForSeconds(releaseTime);
            }
        }

        gameController.AfterPacmomEatenByDust();
    }

    public IEnumerator GetRemaningCoins()
    {
        foreach (Transform coin in coins)
        {
            if (coin.gameObject.activeSelf)
            {
                gameController.soundSystem.PlayEffect("RapleyEatCoin");

                SetRapleyScore(rapleyScore + 1);
                coin.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.03f);
            }
        }
        Invoke("ChooseAWinner", 1.5f);
    }

    private void ChooseAWinner()
    {
        gameController.soundSystem.StopAllSound();

        if (rapleyScore > pacmomScore)
            ending.RapleyWin();
        else
            ending.PacmomWin();
    }

    public bool HasRemainingCoins()
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