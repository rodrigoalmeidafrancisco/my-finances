using Shared.Settings;

namespace TestProject
{
    public static class SetupTests
    {
        public static void Initialize()
        {
            InitializeSettingApp();
        }

        public static void InitializeSettingApp()
        {



            SettingApp.Services = new SettingsAppServices()
            {
                ViaCep = new SettingsAppServices_ViaCep()
                {
                    _LinkBase = "https://viacep.com.br/ws/"
                }
            };
        }
    }
}
