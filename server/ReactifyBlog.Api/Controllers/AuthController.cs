using Microsoft.AspNetCore.Mvc;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Business.DTOs;
using ReactifyBlog.Api.Constants;

namespace ReactifyBlog.Api.Controllers
{
    [ApiController]
    [Route(EndpointRouteConstants.AuthBase)]
    public class AuthController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public AuthController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost(EndpointRouteConstants.AuthRegister)]
        public async Task<ActionResult<AppResponse<bool>>> Register([FromBody] RegisterRequest request)
        {
            var result = await _identityService.RegisterUserAsync(request);
            if (result)
            {
                return Ok(new AppResponse<bool> { Data = true });
            }
            return BadRequest(new AppResponse<bool> { Error = new ErrorInfo { Code = "REG_FAILED", Message = "Registration failed" } });
        }

        [HttpPost(EndpointRouteConstants.AuthLogin)]
        public async Task<ActionResult<AppResponse<bool>>> Login([FromBody] LoginRequest request)
        {
            var result = await _identityService.LoginUserAsync(request);
            if (result)
            {
                return Ok(new AppResponse<bool> { Data = true });
            }
            return Unauthorized(new AppResponse<bool> { Error = new ErrorInfo { Code = "LOGIN_FAILED", Message = "Invalid credentials" } });
        }
    }
}
