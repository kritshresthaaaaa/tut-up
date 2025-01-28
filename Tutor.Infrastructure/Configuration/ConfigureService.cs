using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tutor.Application.Common.Interfaces;
using Tutor.Domain.Entities;
using Tutor.Infrastructure.Configuration.EmailConfiguration;
using Tutor.Infrastructure.Data;

namespace Tutor.Infrastructure.Configuration
{
    public static class ConfigureService
    {
        public static void AddInfrastructureService(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            service.AddHttpContextAccessor();
            service.AddScoped<IUnitOfWork, UnitOfWork>();
            service.AddScoped<DatabaseInterceptor>();
            service.AddDbContext(configuration);
            service.AddIdentity(configuration);
            service.AddJwtConfiguration(configuration);
            service.AddEmailConfiguration(configuration);
            service.AddScoped<IEmailService, EmailService>();
            //generic repository
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
            services.AddDbContext<TutorDbContext>((serviceProvider, options) =>
            {
                options.UseNpgsql(connectionString, builder => builder.MigrationsAssembly(typeof(TutorDbContext).Assembly.FullName));
                options.EnableSensitiveDataLogging();
                // interceptor
                var interceptor = serviceProvider.GetRequiredService<DatabaseInterceptor>();
                options.AddInterceptors(interceptor);
            });
        }
        private static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = (context) =>
                    {
                        context.Token = context.Request.Cookies["access_token"];
                        return Task.CompletedTask;
                    }
                };
            }
            );

        }
        private static void AddEmailConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        }
    }
}
