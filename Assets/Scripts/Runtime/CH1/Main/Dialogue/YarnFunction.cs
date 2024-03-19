using Runtime.InGameSystem;
using Yarn.Unity;

namespace Runtime.CH1.Main.Dialogue
{
    // Yarn Spinner에서 사용할 정적 함수들을 모아놓은 클래스
    public static class YarnFunction
    {
        [YarnFunction("GetMinor")]
        public static int GetMinor()
        {
            return Managers.Data.Minor;
        }
    }
}
