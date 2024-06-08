using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1.Lucky
{
    public class LuckyTrigger : MonoBehaviour
    {
        [SerializeField] private LuckyCH1Dialogue _dialogue;
        [SerializeField] private GameObject _luckyLayer;
        [SerializeField] private string _stageName;
        private bool _luckyExplained3;

        private void Start()
        {
            _luckyExplained3 = false;
        }

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
                else if (!_luckyExplained3 && _stageName == "3Match3")
                {
                    _luckyExplained3 = true;
                    _luckyLayer.SetActive(true);
                    _dialogue.S3ExplainStart();
                }
            }
        }
    }
}