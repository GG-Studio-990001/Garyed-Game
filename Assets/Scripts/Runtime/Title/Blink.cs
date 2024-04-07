using System.Collections;
using UnityEngine;

namespace Runtime.CH1.Title
{
    public class Blink : MonoBehaviour
    {
        [SerializeField]
        private GameObject _target;
        [SerializeField]
        private float _showTime = 1.5f;
        [SerializeField]
        private float _hideTime = 0.5f;

        private void Start()
        {
            StartCoroutine(BlinkTarget());
        }

        private IEnumerator BlinkTarget()
        {
            while (true)
            {
                _target.SetActive(true);
                yield return new WaitForSeconds(_showTime);
                _target.SetActive(false);
                yield return new WaitForSeconds(_hideTime);
            }
        }
    }
}