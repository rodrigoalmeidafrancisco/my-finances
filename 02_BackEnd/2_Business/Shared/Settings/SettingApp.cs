using Microsoft.Extensions.Configuration;

namespace Shared.Settings
{
    /// <summary>
    /// Gerenciador centralizado de configurações da aplicação.
    /// Vincula seções de configuração do appsettings.json a objetos fortemente tipados.
    /// </summary>
    public static class SettingApp
    {
        /// <summary>
        /// Inicializa todas as configurações da aplicação a partir da configuração fornecida.
        /// </summary>
        /// <param name="configuration">A instância de configuração contendo as configurações da aplicação.</param>
        /// <param name="webRootPath">O caminho físico para o diretório raiz da web.</param>
        /// <exception cref="ArgumentNullException">Lançada quando configuration ou webRootPath é nulo.</exception>
        public static void Start(IConfiguration configuration, string webRootPath)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            ArgumentException.ThrowIfNullOrWhiteSpace(webRootPath);

            Aplication = new SettingAppAplication();
            configuration.GetSection("Aplication").Bind(Aplication);

            ApplicationInsights = new SettingAppApplicationInsights();
            configuration.GetSection("ApplicationInsights").Bind(ApplicationInsights);

            ConnectionStrings = new SettingAppConnectionStrings();
            configuration.GetSection("ConnectionStrings").Bind(ConnectionStrings);

            Parameters = new SettingAppParameters();
            configuration.GetSection("Parameters").Bind(Parameters);

            Services = new SettingsAppServices();
            configuration.GetSection("Services").Bind(Services);

            WebRootPath = webRootPath;
            WebRootPathImages = Path.Combine(webRootPath, "images");
        }

        public static SettingAppAplication Aplication { get; private set; } = new();
        public static SettingAppApplicationInsights ApplicationInsights { get; private set; } = new();
        public static SettingAppConnectionStrings ConnectionStrings { get; private set; } = new();
        public static SettingAppParameters Parameters { get; private set; } = new();
        public static SettingsAppServices Services { get; private set; } = new();
        public static string WebRootPath { get; private set; } = string.Empty;
        public static string WebRootPathImages { get; private set; } = string.Empty;
    }
}
