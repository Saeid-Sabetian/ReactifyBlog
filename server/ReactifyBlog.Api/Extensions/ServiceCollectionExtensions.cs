using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReactifyBlog.Business.Constants;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.DTOs;
using ReactifyBlog.Business.Filters;
using ReactifyBlog.Business.MappingProfiles;
using ReactifyBlog.Business.Services;
using ReactifyBlog.Business.Validators.Auth;
using ReactifyBlog.Data.Data;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Api.Extensions
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ReactifyBlogDbContext>(options =>
					options.UseSqlServer(configuration.GetConnectionString(DatabaseConstants.DefaultConnectionStringName)));

			services.AddIdentity<UserDBO, RoleDBO>(options =>
			{
				options.User.RequireUniqueEmail = true;
				options.SignIn.RequireConfirmedEmail = false;
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;
			})
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

		public static IServiceCollection AddFluentValidation(this IServiceCollection services)
		{
			services.AddValidatorsFromAssembly(typeof(RegisterRequestValidator).Assembly);

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.SuppressModelStateInvalidFilter = true;
			});

			return services;
		}

		public static IServiceCollection AddAutoMapper(this IServiceCollection services)
		{
			services.AddAutoMapper(typeof(UserProfile).Assembly);
			return services;
		}

		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IIdentityService, IdentityService>();
			services.AddScoped<IEmailService, EmailService>();

			services.AddScoped<ValidationFilter>();

			return services;
		}
	}
}