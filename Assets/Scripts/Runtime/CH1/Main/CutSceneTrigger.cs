using Runtime.ETC;
using UnityEngine;
using Yarn.Unity;

namespace Runtime.CH1
{
    public class CutSceneTrigger : MonoBehaviour
    {
        [SerializeField] private DialogueRunner dialogueRunner;
        [SerializeField] private int _idx;

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: Switch문으로 변경
            if (_idx == 2)
            {
                if (Managers.Data.Scene != 2)
                    return;

                if (other.CompareTag(GlobalConst.PlayerStr))
                {
                    dialogueRunner.StartDialogue("S3");
                }
            }
            else if (_idx == 3)
            {
                if (Managers.Data.Scene != 3 || Managers.Data.SceneDetail != 0)
                    return;

                if (other.CompareTag(GlobalConst.PlayerStr))
                {
                    dialogueRunner.StartDialogue("S3_1");
                }
            }
            else if (_idx == 4)
            {
                if (Managers.Data.Scene != 3 || Managers.Data.SceneDetail != 1)
                    return;

                if (other.CompareTag(GlobalConst.PlayerStr))
                {
                    dialogueRunner.StartDialogue("S4");
                }
            }
            else if (_idx == 6)
            {
                if (Managers.Data.Scene != 5 || Managers.Data.SceneDetail != 1)
                    return;

                if (other.CompareTag(GlobalConst.PlayerStr))
                {
                    dialogueRunner.StartDialogue("S6");
                }
            }
        }
    }
}