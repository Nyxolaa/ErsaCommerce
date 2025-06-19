using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ErsaCommerce.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(IServiceCollection services)
        {
            services.AddOptions();
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
