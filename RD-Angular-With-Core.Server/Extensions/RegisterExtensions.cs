using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RD.Core.EFContext;
using RD.Core.Factory;
using RD.Core.Repositories.Base;
using RD.Core.Repositories.Interfaces;
using RD.Core.Uow;
using RD.Domain.Identity;
using RD.Services;
using SampleProject.Services;
using System;

namespace RD.API.Extensions
{
    internal static class RegisterExtensions
    {
        internal static void AddDbContexts(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            var contextConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<DatabaseContext>(x => x.UseSqlServer(contextConnectionString, o =>
                {
                    o.EnableRetryOnFailure(3);
                })
                .EnableSensitiveDataLogging(environment.IsDevelopment())
                .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
        }

        internal static void AddInjections(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseContext, DatabaseContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient(typeof(IRDDataServices), typeof(RDDataServices));
            services.AddTransient(typeof(ILogDataServices), typeof(LogDataServices));
            services.AddTransient(typeof(IUsersServices), typeof(UsersServices));
            services.AddScoped<IContextFactory, ContextFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        internal static void AddIdentity(this IServiceCollection services)
        {

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //.AddRoleManager<RoleManager<IdentityRole>>()
            //.AddDefaultTokenProviders()
            //.AddEntityFrameworkStores<DatabaseContext>();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = false;
            });
        }
    }
}
