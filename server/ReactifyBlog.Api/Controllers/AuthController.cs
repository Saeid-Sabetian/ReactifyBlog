using Microsoft.AspNetCore.Mvc;
using ReactifyBlog.Api.Constants;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.DTOs;
using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Business.Filters;

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
			var result = await _identityService.RegisterUserAsync(request);

			if (result)
			{
				return Ok();
			}

			return BadRequest();
		}

		[HttpPost(EndpointRouteConstants.AuthLogin)]
		public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
		{
			var result = await _identityService.LoginUserAsync(request, cancellationToken);

			if (result)
			{
				return Ok();
			}

			return Unauthorized();
		}

    [HttpPost(EndpointRouteConstants.AuthConfirmAccount)]
    public async Task<IActionResult> ConfirmAccount([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
      var result = await _identityService.ConfirmEmailAsync(request, cancellationToken);

      if (result)
      {
        return Ok();
      }

      return Unauthorized();
    }
  }
}
