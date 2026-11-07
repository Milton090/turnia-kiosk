using Services.Constants;

namespace TurniaKiosk.MauiServices
{
    public class AppInitializationService
    {
        public static bool ApplyColorCustomizations(string primaryColorRgbString)
        {
            var errorFlag = false;

            var mykey = Application.Current?.Resources.ContainsKey(ConstantDefinitions.ResDynamicBackgroundColor);
            if (mykey == true && Application.Current != null)
            {
                Application.Current.Resources[ConstantDefinitions.ResDynamicBackgroundColor] = Color.FromArgb(primaryColorRgbString);
            }
            else
            {
                errorFlag = true;
            }

            return errorFlag;
        }
    }
}