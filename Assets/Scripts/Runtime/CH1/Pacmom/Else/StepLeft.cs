using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class StepLeft : MonoBehaviour
    {
        private Step step;

        private void Start()
        {
            step = GetComponent<Step>();

            for (int i = 0; i < 2; i++)
                step.availableDirections.Add(Vector2.left); // 3/5 확률
        }
    }
}