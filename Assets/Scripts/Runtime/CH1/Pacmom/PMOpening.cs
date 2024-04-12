using Runtime.ETC;
using UnityEngine;
using DG.Tweening;

namespace Runtime.CH1.Pacmom
{
    public class PMOpening : MonoBehaviour
    {
        private ScreenResolution _screenResolution;
        private int _normalPPU;
        private int _zoomOutPPU;
        private Tween _ppuTween = null;

        private void Awake()
        {
            _screenResolution = GetComponent<ScreenResolution>();
            ResetPPU();
        }

        private void Update()
        {
            if (_ppuTween is null || !_ppuTween.active)
            {
                ResetPPU();
            }

            if (IsWindowed())
                _screenResolution._windowedPPU = _normalPPU;
            else
                _screenResolution._fullScreenPPU = _normalPPU;
        }

        private void ResetPPU()
        {
            _normalPPU = IsWindowed() ? _screenResolution._windowedPPU : _screenResolution._fullScreenPPU;
            _zoomOutPPU = IsWindowed() ? 21 : 32;
        }

        private bool IsWindowed()
        {
            return Screen.width == 1280;
        }

        public void ZoomOut()
        {
            float duration = 1f;
            _ppuTween = DOTween.To(() => _normalPPU, x => _normalPPU = x, _zoomOutPPU, duration).OnComplete(() => FixPPU());
            _ppuTween.Play();
        }

        private void FixPPU()
        {
            _screenResolution._windowedPPU = 21;
            _screenResolution._fullScreenPPU = 32;

            this.enabled = true;
        }
    }
}