using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class StepLeft : MonoBehaviour
    {
        private Step step;

        private void Start()
        {
            step = GetComponent<Step>();

            step.AddStepsTwice(step, Vector2.left);
        }
    }
}