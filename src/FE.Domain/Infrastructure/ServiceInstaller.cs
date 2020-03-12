using FE.Domain.Configuration;
using FE.Domain.Facades;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FE.Domain.Infrastructure
{
    public static class ServiceInstaller
    {
        public static IServiceCollection InstallFundaServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FundaConfiguration>(conf => configuration.Bind("Funda", conf));
            services.AddTransient<IFundaFacade, FundaFacade>();
            services.AddTransient<ITopTenService, TopTenService>();

            return services;
        }
    }
}
