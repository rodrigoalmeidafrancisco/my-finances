namespace Shared.Settings
{
    public class SettingsAppServices
    {
        public SettingsAppServices()
        {
            ViaCep = new SettingsAppServices_ViaCep();
        }

        public SettingsAppServices_ViaCep ViaCep { get; set; }
    }

    public class SettingsAppServices_ViaCep
    {
        public string _LinkBase { get; set; }
    }
}
