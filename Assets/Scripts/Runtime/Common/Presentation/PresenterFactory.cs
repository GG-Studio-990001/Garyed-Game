using Runtime.Common.View;
using Runtime.Data;
using Runtime.InGameSystem;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Runtime.Common.Presentation
{
    public static class PresenterFactory
    {
        public static SettingsUIPresenter CreateSettingsUIPresenter(SettingsUIView view)
        {
            return new SettingsUIPresenter(view, DataProviderManager.Instance.SettingsDataProvider.Get());
            //return new SettingsUIPresenter(view, Resources.Load<SettingsData>("SettingsData"));
        }
    }
}