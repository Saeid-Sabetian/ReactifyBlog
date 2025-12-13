using Microsoft.AspNetCore.Http;

namespace ReactifyBlog.Business.Contracts.Wrappers;

public interface IHttpContextAccessorWrapper
{
  string? GetClaim(string claim);
  bool IsAuthenticated();
  void AddCookie(string cookieName, string cookieValue, CookieOptions? cookieOptions);
  string? GetCookieValue(string cookieName);
  bool RemoveCookie(string cookieName, CookieOptions cookieOptions);
}
