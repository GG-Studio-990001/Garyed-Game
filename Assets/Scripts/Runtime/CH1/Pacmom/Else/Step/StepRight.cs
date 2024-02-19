using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class StepRight : MonoBehaviour
    {
        private Step step;

        private void Start()
        {
            step = GetComponent<Step>();

            step.AddStepsTwice(step, Vector2.right);
        }
    }
}