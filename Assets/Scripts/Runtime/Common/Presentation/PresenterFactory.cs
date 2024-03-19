using Runtime.Common.View;
using Runtime.InGameSystem;

namespace Runtime.Common.Presentation
{
    public static class PresenterFactory
    {
        public static SettingsUIPresenter CreateSettingsUIPresenter(SettingsUIView view)
        {
            return new SettingsUIPresenter(view);
        }
    }
}