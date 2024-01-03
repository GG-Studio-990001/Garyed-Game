using Runtime.Data.Original;
using Runtime.Interface;

namespace Runtime.Data.Provider
{
    public class ControlsDataProvider : IProvider<ControlsData>
    {
        private readonly ControlsData _controlsData;
        
        public ControlsDataProvider(ControlsData controlsData)
        {
            _controlsData = controlsData;
        }
        
        /// <summary>
        /// 컨트롤러는 Deep Copy를 하지 않는다.
        /// </summary>
        /// <returns></returns>
        public ControlsData Get()
        {
            return _controlsData;
        }

        public void Set(ControlsData value)
        {
            return;
        }
    }
}