using Runtime.ETC;
using Runtime.Luck;
using UnityEngine;
using Yarn.Unity;

public class LuckyTrigger : MonoBehaviour
{
    [SerializeField] private Lucky3MatchDialogue _dialogue;
    [SerializeField] private GameObject _luckyLayer;
    [SerializeField] private string _stageName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GlobalConst.PlayerStr))
        {
            // TODO: 첫 럭키면 미리 했을 때의 분기점 추가
            if (_stageName == "3Match1")
            {
                _luckyLayer.SetActive(true);
                _dialogue.S1ExplainStart();
            }
            else if (_stageName == "3Match3")
            {
                _luckyLayer.SetActive(true);
                _dialogue.S3ExplainStart();
            }
        }
    }
}
