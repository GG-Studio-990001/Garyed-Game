using Runtime.CH1.Main.Dialogue;
using Runtime.ETC;
using UnityEngine;

namespace Runtime.CH1
{
    public class CutSceneTrigger : MonoBehaviour
    {
        [SerializeField] private Ch1DialogueController dialogue;

        /*private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(GlobalConst.PlayerStr))
            {
                dialogue.CheckCutScene();
            }
        }*/
    }
}