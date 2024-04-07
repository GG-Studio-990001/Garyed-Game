using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Runtime.CH1.Pacmom
{
    public class ScreenResolustion : MonoBehaviour
    {
        PixelPerfectCamera _pixCam;
        private int _normalPPU;
        private int _zoomedPPU;

        private void Awake()
        {
            _pixCam = GetComponent<PixelPerfectCamera>();

            int width = (1920 >= Screen.width ? 1920 : Screen.width);
            _normalPPU = width / 60;
            _zoomedPPU = (int)(width / 21.34f);
        }

        //public void getnormalppu()
        //{
        //    _pixcam.assetsppu = _normalppu;
        //}

        public void GetZoomedPPU()
        {
            _pixCam.assetsPPU = _zoomedPPU;
        }

        public void ZoomOut()
        {
            StartCoroutine(ZoomOutCamera());
        }

        private IEnumerator ZoomOutCamera() // DOTween으로 변경 예정
        {
            float time = 1f;
            float holeTime = time;

            while (time >= 0)
            {
                time -= Time.deltaTime;

                _pixCam.assetsPPU = (int)((_zoomedPPU * (time / holeTime)) + (_normalPPU * (1 - time / holeTime)));
                float yPos = (-0.43f * (time / holeTime)) + (-0.7f * (1 - time / holeTime));
                transform.localPosition = new Vector3(0, yPos, -10);

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
    }
}