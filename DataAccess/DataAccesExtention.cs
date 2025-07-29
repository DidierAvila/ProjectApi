using DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class DataAccesExtention
    {
        public static IServiceCollection AddDataAccesExtention(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepositoy, CustomerRepositoy>();
            services.AddScoped<IPostRepository, PostRepository>();
            return services;
        }
    }
}
