using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tutor.Application.Common.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Infrastructure.Data;

namespace Tutor.Infrastructure.Configuration
{
    public static class ConfigureService
    {
        public static void AddInfrastructureService(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext(configuration);
            service.AddIdentity(configuration);
            //generic repository
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
        private static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                options.Tokens.EmailConfirmationTokenProvider = "Email";
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(double.Parse(configuration["LockoutConfiguration:TimeInMinutes"]));
                options.Lockout.MaxFailedAccessAttempts = int.Parse(configuration["LockoutConfiguration:MaxAttempt"]);
                options.Lockout.AllowedForNewUsers = true;
                options.User.RequireUniqueEmail = true;
            })
           .AddEntityFrameworkStores<TutorDbContext>()
           .AddDefaultTokenProviders()
           .AddRoles<Role>();
        }
        private static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TutorDbContext>(options =>
            {
                options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(typeof(TutorDbContext).Assembly.FullName));
                options.EnableSensitiveDataLogging();
                // interceptor
            });
        }
    }
}
