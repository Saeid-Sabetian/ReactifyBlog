using Microsoft.AspNetCore.Mvc;
using ReactifyBlog.Api.Constants;
using ReactifyBlog.Api.Filters;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.DTOs.Auth;

namespace ReactifyBlog.Api.Controllers
{
  [ApiController]
  [ServiceFilter(typeof(ValidationFilter))]
  [Route(EndpointRouteConstants.AuthBase)]
  public class AuthController : ControllerBase
  {
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
      _identityService = identityService;
    }

    [HttpPost(EndpointRouteConstants.AuthRegister)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
      await _identityService.RegisterUserAsync(request);

      return Ok();
    }

    [HttpPost(EndpointRouteConstants.AuthLogin)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
      await _identityService.LoginUserAsync(request, cancellationToken);

      return Ok();
    }

    [HttpPost(EndpointRouteConstants.AuthConfirmAccount)]
    public async Task<IActionResult> ConfirmAccount([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
      await _identityService.ConfirmEmailAsync(request, cancellationToken);

      return Ok();
    }
  }
}
