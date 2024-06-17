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
            if (Managers.Data.Scene > 3 || (Managers.Data.Scene == 3 && Managers.Data.SceneDetail == 1))
                return;

            if (other.CompareTag(GlobalConst.PlayerStr))
            {
                // TODO: 럭키 없이 미리 클리어시 분기점 추가
                // TODO: 동굴 막기 없앨 시 Managers.Data.MeetLucky 조건 추가
                if (_stageNum == 1 && !Managers.Data.Is3MatchEntered)
                {
                    Managers.Data.Is3MatchEntered = true;
                    _luckyDialogue.StartDialogue("Lucky3Match");
                }
                else if (_stageNum == 3 && !_luckyExplained3 && !Managers.Data.Is3MatchCleared)
                {
                    // TODO: 대화 끝날 때 true 처리
                    _luckyExplained3 = true;
                    _luckyDialogue.StartDialogue("LuckyFish");
                }
            }
        }
    }
}