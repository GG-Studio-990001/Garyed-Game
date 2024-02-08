using UnityEngine;

namespace Runtime.CH1.Pacmom
{
    public class DoorStep : MonoBehaviour
    {
        private Step step;

        private void Start()
        {
            step = GetComponent<Step>();

            step.availableDirections.Clear();
            step.availableDirections.Add(Vector2.up);
        }
    }
}