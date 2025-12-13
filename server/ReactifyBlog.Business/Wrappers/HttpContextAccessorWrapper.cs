using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ReactifyBlog.Business.Contracts.Wrappers;

namespace ReactifyBlog.Business.Wrappers;

public class HttpContextAccessorWrapper : IHttpContextAccessorWrapper
{
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IConfiguration _configuration;
  public HttpContextAccessorWrapper(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
  {
    _httpContextAccessor = httpContextAccessor;
    _configuration = configuration;
  }
  public string? GetClaim(string claim)
  {
    var user = _httpContextAccessor.HttpContext?.User;

    if (user is null)
    {
      return null;
    }


    return user.FindFirst(claim)?.Value;
  }

  public bool IsAuthenticated()
  {
    return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
  }
  public void AddCookie(string cookieName, string cookieValue, CookieOptions? cookieOptions)
  {
    _httpContextAccessor.HttpContext?.Response.Cookies.Append(cookieName, cookieValue, cookieOptions);
  }

  public string? GetCookieValue(string cookieName)
  {
    var context = _httpContextAccessor.HttpContext;
    if (context == null)
    {
      return null;
    }

    context.Request.Cookies.TryGetValue(cookieName, out string? cookieValue);

    return cookieValue;
  }

  public bool RemoveCookie(string cookieName, CookieOptions cookieOptions)
  {
    var context = _httpContextAccessor.HttpContext;
    if (context == null) return false;

    if (cookieOptions is not null)
    {
      var cookieOptionsToRemove = new CookieOptions()
      {
        HttpOnly = cookieOptions.HttpOnly,
        Secure = cookieOptions.Secure,
        SameSite = cookieOptions.SameSite,
        Expires = DateTimeOffset.UtcNow.AddDays(-1),
        Path = cookieOptions.Path,
      };
      cookieOptions.Expires = DateTimeOffset.UtcNow.AddDays(-1);
    }

    context.Response.Cookies.Delete(cookieName);
    return true;
  }
}
