using Business.Customers.Queries;
using Business.Logs;
using Business.Posts.Queries;
using Business.Security;
using DataAccess.Repositories;
using Domain.Entities;
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

            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IRepositoryBase<Token>, RepositoryBase<Token>>();
            services.AddScoped<IRepositoryBase<User>, RepositoryBase<User>>();
            services.AddScoped<IRepositoryBase<Logs>, RepositoryBase<Logs>>();

            return services;
        }
    }
}
