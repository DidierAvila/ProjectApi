using Business.Customers.Queries;
using Business.Posts.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApiExtension
    {
        public static IServiceCollection AddApiExtention(this IServiceCollection services)
        {
            // Registrar MediatR para Customer
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetAllCustomersQuery).Assembly));
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetCustomerByIdQuery).Assembly));
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetCustomerPostsQuery).Assembly));

            // Registrar MediatR para Post
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetAllPostsQuery).Assembly));
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetPostByIdQuery).Assembly));
            services.AddMediatR(config => config.RegisterServicesFromAssembly(typeof(GetPostsByCustomerIdQuery).Assembly));

            return services;
        }
    }
}
