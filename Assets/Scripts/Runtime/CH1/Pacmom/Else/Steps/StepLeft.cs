using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class StepLeft : MonoBehaviour
    {
        private Step _step;

        private void Start()
        {
            _step = GetComponent<Step>();

            _step.AddStepsTwice(_step, Vector2.left);
        }
    }
}