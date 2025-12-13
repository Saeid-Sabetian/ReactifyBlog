using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReactifyBlog.Api.Filters;
using ReactifyBlog.Business.Constants;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.Contracts.Wrappers;
using ReactifyBlog.Business.MappingProfiles;
using ReactifyBlog.Business.Services;
using ReactifyBlog.Business.Validators.Auth;
using ReactifyBlog.Business.Wrappers;
using ReactifyBlog.Data.Data;
using ReactifyBlog.Data.Models;

namespace ReactifyBlog.Api.Extensions;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddDbContext<ReactifyBlogDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString(DatabaseConstants.DefaultConnectionStringName)));

    services.AddIdentity<UserDBO, RoleDBO>(options =>
    {
      options.User.RequireUniqueEmail = true;
      options.SignIn.RequireConfirmedEmail = true;
      options.Password.RequireDigit = true;
      options.Password.RequiredLength = NumericConstants.Six;
      options.Password.RequireNonAlphanumeric = true;
      options.Password.RequireUppercase = true;
      options.Password.RequireLowercase = true;
      options.Lockout.MaxFailedAccessAttempts = NumericConstants.Five;
      options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(NumericConstants.Fifteen);
      options.Lockout.AllowedForNewUsers = true;
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
      options.ExpireTimeSpan = TimeSpan.FromMinutes(NumericConstants.Twenty);
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
    services.AddAutoMapper(typeof(MapperProfile).Assembly);
    return services;
  }

  public static IServiceCollection AddApplicationServices(this IServiceCollection services)
  {
    services.AddScoped<IAuthenticationService, AuthenticationService>();
    services.AddScoped<IEmailService, EmailService>();
    services.AddScoped<IHttpContextAccessorWrapper, HttpContextAccessorWrapper>();

    services.AddScoped<ValidationFilter>();

    return services;
  }
}