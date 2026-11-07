namespace Services.Constants
{
    public static class ConstantDefinitions
    {
        #region Maui Preferences Keys
        public const string MauiPrefAppUrl = "PrefAppUrl";
        public const string MauiPrefDeviceKey = "PrefDeviceKey";
        #endregion

        public const string DeviceKey = "turnia-kiosco-demo";
        public const string DeviceKeyHeaderDefinition = "Turnia-DeviceKey";

        #region Maui Color customization keys
        public const string ResDynamicBackgroundColor = "DynamicBackgroundColor";

        #endregion


        #region BaseUri and External API Routes
#if DEBUG
        public const string BaseUrl = "http://localhost:6101";
#else
        public const string BaseUrl = "https://turnia-api.onrender.com";
#endif
        public const string RouteGetDeviceCheckinConfig ="/devices/checkindevice/checkindeviceconfigurations";
        public const string RouteGetServices = "/devices/checkindevice/services";
        public const string RouteCreateTicket = "/devices/checkindevice/tickets";
        public const string RouteMarkTicketAsPrinted = "/devices/checkindevice/tickets/{id}/markprinted";
        #endregion

        #region ColorCustomization
        public const string PrimaryColor = "#0F766E";
        public const string PrimaryTextColor = "#ffffff";
        public const string PrimaryLabelColor = "#1F2937";
        public const string ScreenBackgroundColor = "#F3F4F6";


        #endregion

        #region Selection paging (4 rows x 2 columns)
        public const int ServicesPerPage = 8;
        #endregion
    }
}
