using CustomerAPI.Models;
using CustomerAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerAPI.Injections
{
    public static class RepositoryInjection
    {
        public static IServiceCollection AddRepositoryConfiguration(this IServiceCollection services)
        {
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase(databaseName: "InMemoryCustomerDb"),
               ServiceLifetime.Scoped,
               ServiceLifetime.Scoped);

            return services;
        }
    }
}
