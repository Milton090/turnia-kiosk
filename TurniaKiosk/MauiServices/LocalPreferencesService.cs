using Services.Constants;

namespace TurniaKiosk.MauiServices
{
    public static class LocalPreferencesService
    {
        #region App Url
        public static string GetAppUrl()
        {
            return Preferences.Default.Get<string>(ConstantDefinitions.MauiPrefAppUrl,ConstantDefinitions.BaseUrl);
        }

        public static bool SetAppUrl(string appUrl)
        {
            Preferences.Default.Set<string>(ConstantDefinitions.MauiPrefAppUrl, appUrl);
            var storedAppUrl = GetAppUrl();

            if (string.Compare(storedAppUrl, appUrl, false) == 0)
            {
                return true;
            }
            return false;
        }
        #endregion


        #region Device Key (api key)

        public static string? GetDeviceKey()
        {
            return Preferences.Default.Get<string>(nameof(ConstantDefinitions.MauiPrefDeviceKey), string.Empty);
        }

        public static bool SetDeviceKey(string deviceKey)
        {
            Preferences.Default.Set<string>(nameof(ConstantDefinitions.MauiPrefDeviceKey), deviceKey);
            var storedDeviceKey = GetDeviceKey();

            if (string.Compare(storedDeviceKey, deviceKey, false) == 0)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}