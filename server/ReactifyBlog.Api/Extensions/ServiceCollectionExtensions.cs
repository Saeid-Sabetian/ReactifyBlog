using ReactifyBlog.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ReactifyBlog.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.OpenApi.Models;
using ReactifyBlog.Business.Constants;
using Microsoft.AspNetCore.Http;

namespace ReactifyBlog.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ReactifyBlogDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(DatabaseConstants.DefaultConnectionStringName)));
            
            services.AddIdentity<ReactifyBlogUser, ReactifyBlogRole>()
                .AddEntityFrameworkStores<ReactifyBlogDbContext>()
                .AddDefaultTokenProviders();
            
            return services;
        }

        public static IServiceCollection AddApplicationCookie(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = CookieConstants.IdentityCookieName;
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
            });
            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(SwaggerConstants.ApiDocName, new OpenApiInfo
                {
                    Title = SwaggerConstants.ApiTitle,
                    Version = SwaggerConstants.ApiVersion
                });
            });
            return services;
        }
    }
}
