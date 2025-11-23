using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReactifyBlog.Business.Constants;
using ReactifyBlog.Business.DTOs;
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

			services.AddIdentity<ReactifyBlogUser, ReactifyBlogRole>(options =>
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
			services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
			services.AddValidatorsFromAssemblyContaining<ReactifyBlog.Business.Validators.Auth.RegisterRequestValidator>();

			services.Configure<ApiBehaviorOptions>(options =>
			{
				options.InvalidModelStateResponseFactory = context =>
							{
								var errors = context.ModelState.Where(e => e.Value?.Errors.Any() == true)
													.ToDictionary(
															kvp => kvp.Key,
															kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
													);

								var errorInfo = new ErrorInfo
								{
									Code = "VALIDATION_FAILED",
									Message = "One or more validation errors occurred.",
									ValidationErrors = errors.ToDictionary(k => k.Key, v => v.Value)
								};

								var appResponse = new AppResponse<object>
								{
									Data = null,
									Error = errorInfo
								};

								return new BadRequestObjectResult(appResponse);
							};
			});

			return services;
		}
	}
}