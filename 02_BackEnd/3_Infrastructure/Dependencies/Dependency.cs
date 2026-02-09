using Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Settings;

namespace Dependencies
{
    public static class Dependency
    {
        //services.AddDbContext -> já faz a mesma função do AddScoped.
        //services.AddSingleton -> Provem uma intancia do objeto para aplicação toda, sempre ativa.
        //services.AddScoped -> Busca sempre da memoria caso exista, se não cria.
        //services.AddTransient -> Cada requisição cria uma nova instância.

        public static void Start(IServiceCollection services)
        {
            Contexts(services);
            Domain(services);
            Data(services);
            DataServices(services);
        }

        private static void Contexts(IServiceCollection services)
        {
            /**********************************************************************************************************************
            ATENÇÃO: Deixar em ordem alfabética 
            **********************************************************************************************************************/

            services.AddDbContext<ContextDefault>(options => options.UseSqlServer(SettingApp.ConnectionStrings.Default)
              .UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()))
           );
        }

        private static void Domain(IServiceCollection services)
        {
            //services.AddScoped<IHandler..., Handler...>();
        }

        private static void Data(IServiceCollection services)
        {
            //services.AddScoped<IRepository..., Repository...>();
        }

        private static void DataServices(IServiceCollection services)
        {
            //services.AddScoped<Ixyz, xyz>();
        }

    }
}
