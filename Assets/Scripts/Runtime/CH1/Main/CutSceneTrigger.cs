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
            if (!other.CompareTag(GlobalConst.PlayerStr))
                return;

            switch (_idx)
            {
                case 1:
                    if (Managers.Data.Scene == 1 && Managers.Data.SceneDetail == 0)
                        dialogueRunner.StartDialogue("S1_1");
                    break;
                case 2:
                    if (Managers.Data.Scene == 2)
                        dialogueRunner.StartDialogue("S3");
                    break;
                case 3:
                    if (Managers.Data.Scene == 3 && Managers.Data.SceneDetail == 0)
                        dialogueRunner.StartDialogue("S3_1");
                    break;
                case 4:
                    if (Managers.Data.Scene == 3 && Managers.Data.SceneDetail == 1)
                        dialogueRunner.StartDialogue("S4");
                    break;
                case 6:
                    if (Managers.Data.Scene == 5 && Managers.Data.SceneDetail == 1)
                        dialogueRunner.StartDialogue("S6");
                    break;
                default:
                    Debug.LogError("Invalid CutSceneTrigger Idx");
                    break;
            }
        }
    }
}