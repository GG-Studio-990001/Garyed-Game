using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Runtime.ETC
{
    public class ScreenResolution : MonoBehaviour
    {
        private PixelPerfectCamera _pixCam;

        private readonly int _maxWidth = 1920;
        private readonly int _maxHeight = 1080;

        private readonly int _windowedWidth = 1280;
        private readonly int _windowedHeight = 720;

        private bool _fullscreenFixed = false;
        private bool _windowedFixed = false;
        private bool _wasFullScreen = false;

        private void Awake()
        {
            _pixCam = GetComponent<PixelPerfectCamera>();
        }

        void Start()
        {
            Screen.SetResolution(1280, 720, false);
        }

        private void LateUpdate()
        {
            if (Screen.fullScreen == true)
            {
                _pixCam.assetsPPU = 100;

                _wasFullScreen = true;
                _windowedFixed = false;

                if (_fullscreenFixed == false)
                {
                    _fullscreenFixed = true;

                    Screen.SetResolution(_maxWidth, _maxHeight, true);
                }
            }
            else
            {
                _pixCam.assetsPPU = 67;

                if (_wasFullScreen == true)
                {
                    _wasFullScreen = false;

                    if (_windowedFixed == false)
                    {
                        _windowedFixed = true;
                        Screen.SetResolution(_windowedWidth, _windowedHeight, false);
                    }
                }

                _fullscreenFixed = false;
            }
        }
    }
}