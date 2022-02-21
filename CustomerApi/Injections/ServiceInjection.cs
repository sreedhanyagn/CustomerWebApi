using CustomerAPI.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerAPI.Injections
{
    public static class ServiceInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<ICustomerService, CustomerService>();
        }
     }
}
