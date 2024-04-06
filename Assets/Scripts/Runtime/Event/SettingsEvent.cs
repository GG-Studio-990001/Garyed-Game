using System;

namespace Runtime.Event
{
    public static class SettingsEvent
    {
        public static event Action<bool> OnSettingsToggled;
        
        public static void ToggleSettings(bool isSettingsOpen)
        {
            OnSettingsToggled?.Invoke(isSettingsOpen);
        }
    }
}