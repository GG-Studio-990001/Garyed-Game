using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Title
{
    public class Blink : MonoBehaviour
    {
        [SerializeField]
        private GameObject _target;
        [SerializeField]
        private float _blinkTime = 1f;

        private void Start()
        {
            StartCoroutine(BlinkTarget());
        }

        private IEnumerator BlinkTarget()
        {
            while (true)
            {
                _target.SetActive(true);
                yield return new WaitForSeconds(_blinkTime);
                _target.SetActive(false);
                yield return new WaitForSeconds(_blinkTime);
            }
        }
    }
}