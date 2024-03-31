using UnityEngine;

namespace Runtime.CH1.Title
{
    public class ObjectController : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _backgrounds = new Transform[2];
        [SerializeField]
        private float _speed = 10;
        private float _gap;
        private readonly float _yPos = 1827;

        private void Start()
        {
            _gap = _backgrounds[0].localPosition.y - _backgrounds[1].localPosition.y;
        }

        private void Update()
        {
            for (int i = 0; i < _backgrounds.Length; i++)
            {
                _backgrounds[i].localPosition += new Vector3(0, _speed, 0) * Time.deltaTime;

                if (_backgrounds[i].localPosition.y > _yPos)
                {
                    Debug.Log("지남");
                    Vector3 nextPos = _backgrounds[1 - i].localPosition;
                    nextPos.y -= _gap;
                    _backgrounds[i].localPosition = nextPos;
                }
            }
        }
    }
}