using Runtime.ETC;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1.Lucky
{
    public class Lucky3MatchTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueRunner _luckyDialogue;
        [SerializeField] private int _stageNum;
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
                if (_stageNum == 1)
                {
                    _luckyDialogue.StartDialogue("Lucky_3Match");
                }
                else if (!_luckyExplained3 && _stageNum == 3)
                {
                    // TODO: 대화 끝날 때 true 처리
                    _luckyExplained3 = true;
                    _luckyDialogue.StartDialogue("Lucky_3Match_2");
                }
            }
        }
    }
}