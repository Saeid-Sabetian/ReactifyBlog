using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ReactifyBlog.Business.Common;
using ReactifyBlog.Business.Constants;
using ReactifyBlog.Business.Constants.ErrorConstants.Exceptions;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Business.Exceptions;
using ReactifyBlog.Data.Data;
using ReactifyBlog.Data.Models;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace ReactifyBlog.Business.Services
{
  public class IdentityService : IIdentityService
  {
    private readonly UserManager<UserDBO> _userManager;
    private readonly SignInManager<UserDBO> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ReactifyBlogDbContext _dbContext;
    private readonly IMapper _mapper;

    public IdentityService(
    UserManager<UserDBO> userManager,
    SignInManager<UserDBO> signInManager,
    IEmailService emailService,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor,
    ReactifyBlogDbContext dbContext
    )
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _emailService = emailService;
      _httpContextAccessor = httpContextAccessor;
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<bool> RegisterUserAsync(RegisterRequest request)
    {
      var user = _mapper.Map<UserDBO>(request);

      var result = await _userManager.CreateAsync(user, request.Password);

      if (result.Succeeded)
      {
        string sixDigitCodeToVerifyEmail = NumericHelper.GenerateRandomInt(NumericConstants.OneHundredThousand, NumericConstants.OneMillion).ToString();

        user.ConfirmEmailCode = sixDigitCodeToVerifyEmail;

        user.ConfirmEmailExpiration = DateTime.UtcNow.AddMinutes(NumericConstants.Fifteen);

        await _userManager.UpdateAsync(user);

        await _emailService.SendEmailAsync(user.Email, EmailConstants.ConfirmEmailSubject, string.Format(EmailConstants.ConfirmEmailMessage, sixDigitCodeToVerifyEmail));

        await _userManager.AddToRoleAsync(user, Data.Constants.RoleConstants.UserRole);

        return true;
      }

      return false;
    }

    public async Task LoginUserAsync(LoginRequest request, CancellationToken cancellationToken)
    {
      var isLoginSuccessful = false;

      var user = await _userManager.FindByEmailAsync(request.Email);

      if (user is null)
      {
        throw new ReactifyBlogException(
          AuthServiceErrorConstants.InvalidCredentialsErrorCode,
          nameof(IdentityService),
          (int)System.Net.HttpStatusCode.BadRequest,
          AuthServiceErrorConstants.InvalidCredentialsErrorMessage);
      }

      if (!user.EmailConfirmed)
      {
        throw new ReactifyBlogException(
                  AuthServiceErrorConstants.UserNotConfirmedErrorCode,
                  nameof(IdentityService),
                  (int)System.Net.HttpStatusCode.BadRequest,
                  AuthServiceErrorConstants.UserNotConfirmedErrorMessage);
      }

      if (user.LockoutEnd.HasValue)
      {
        var utcNow = DateTime.UtcNow;
        if (user.LockoutEnd.Value > utcNow)
        {
          throw new ReactifyBlogException(
                  AuthServiceErrorConstants.LockedOutErrorCode,
                  nameof(IdentityService),
                  (int)System.Net.HttpStatusCode.BadRequest,
                  AuthServiceErrorConstants.LockedOutErrorMessage);
        }

        user.LockoutEnd = null;
        await _userManager.UpdateAsync(user);
      }

      var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
      if (result.Succeeded)
      {
        await _signInManager.SignInAsync(user, false);

        if (request.RememberMe)
        {
          await GenerateAndStoreRefreshTokenAsync(user, cancellationToken);
        }

        isLoginSuccessful = true;
      }

      if (!isLoginSuccessful)
      {
        throw new ReactifyBlogException(
                  AuthServiceErrorConstants.LoginFailedErrorCode,
                  nameof(IdentityService),
                  (int)System.Net.HttpStatusCode.BadRequest,
                  AuthServiceErrorConstants.LoginFailedErrorMessage);
      }
    }

    public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
      var user = await _userManager.FindByIdAsync(request.UserId.ToString());
      if (user == null)
      {
        throw new ReactifyBlogException(AuthServiceErrorConstants.ChangePasswordFailedErrorCode, nameof(IdentityService), (int)System.Net.HttpStatusCode.NotFound, AuthServiceErrorConstants.ChangePasswordFailedErrorMessage);
      }

      var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
      return result.Succeeded;
    }

    public async Task<bool> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken)
    {
      var user = await _userManager.FindByIdAsync(request.UserId.ToString());
      if (user == null)
      {
        return false;
      }

      user.NickName = request.NickName;
      var result = await _userManager.UpdateAsync(user);
      return result.Succeeded;
    }

    public async Task<bool> RecoverPasswordAsync(RecoverPasswordRequest request, CancellationToken cancellationToken)
    {
      var user = await _userManager.FindByEmailAsync(request.Email);
      if (user == null)
      {
        return false;
      }

      string sevenDigitCode = GenerateSecureSevenDigitCodeToVerifyEmail();
      user.ConfirmEmailCode = sevenDigitCode;
      user.ConfirmEmailExpiration = DateTimeOffset.UtcNow.AddMinutes(15);
      await _userManager.UpdateAsync(user);

      if (user.Email != null)
      {
        await _emailService.SendEmailAsync(user.Email, EmailConstants.RecoverPasswordSubject, $"Your password recovery code is: {sevenDigitCode}");
      }
      return true;
    }

    public async Task ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
      var user = await _userManager.FindByEmailAsync(request.Email.Trim());

      if (user is null)
      {
        throw new ReactifyBlogException(
                  AuthServiceErrorConstants.ConfirmEmailUserDoesNotExistErrorCode,
                  nameof(IdentityService),
                  (int)System.Net.HttpStatusCode.NotFound,
                  AuthServiceErrorConstants.ConfirmEmailUserDoesNotExistErrorMessage);
      }

      if (user.EmailConfirmed)
      {
        throw new ReactifyBlogException(
              AuthServiceErrorConstants.ConfirmEmailAlreadyConfirmedErrorCode,
              nameof(IdentityService),
              (int)System.Net.HttpStatusCode.NotAcceptable,
              AuthServiceErrorConstants.ConfirmEmailAlreadyConfirmedErrorMessage);
      }

      var utcNow = DateTime.UtcNow;
      if (utcNow > user.ConfirmEmailExpiration)
      {
        throw new ReactifyBlogException(
              AuthServiceErrorConstants.ConfirmEmailExpiredErrorCode,
              nameof(IdentityService),
              (int)System.Net.HttpStatusCode.BadRequest,
              AuthServiceErrorConstants.ConfirmEmailExpiredErrorMessage);
      }

      if (!request.ConfirmationCode.Equals(user.ConfirmEmailCode))
      {
        throw new ReactifyBlogException(
                  AuthServiceErrorConstants.ConfirmEmailInvalidCodeErrorCode,
                  nameof(IdentityService),
                  (int)System.Net.HttpStatusCode.BadRequest,
                  AuthServiceErrorConstants.ConfirmEmailInvalidCodeErrorMessage);
      }

      user.EmailConfirmed = true;
      user.ConfirmEmailCode = null;
      user.ConfirmEmailExpiration = null;
      await _userManager.UpdateAsync(user);
    }

    public async Task<bool> LogoutUserAsync()
    {
      await _signInManager.SignOutAsync();
      return true;
    }

    public async Task RefreshTokenAsync(string refreshToken)
    {
      // In a real application, you would validate the refresh token against a stored token (e.g., in a database).
      // If valid, you would revoke the old refresh token, generate new authentication and refresh tokens,
      // and issue a new authentication cookie.
      // For this example, we'll just re-sign in the user if the refresh token is considered valid (e.g., by checking a mock store).
      // This part requires a proper refresh token storage and validation mechanism.
      await Task.CompletedTask;
      throw new NotImplementedException("Refresh token logic needs to be implemented with proper token storage and validation.");
    }

    private async Task GenerateAndStoreRefreshTokenAsync(UserDBO user, CancellationToken cancellationToken)
    {
      var randomNumber = new byte[64];
      using var rng = RandomNumberGenerator.Create();
      rng.GetBytes(randomNumber);
      var refreshToken = Convert.ToBase64String(randomNumber);

      var hashedRefreshToken = _userManager.PasswordHasher.HashPassword(user, refreshToken);

      var refreshTokenDBO = new RefreshTokenDBO
      {
        TokenHash = hashedRefreshToken,
        UserId = user.Id,
        ExpiresAt = DateTime.UtcNow.AddDays(7),
        CreatedAt = DateTime.UtcNow
      };

      await _dbContext.RefreshTokens.AddAsync(refreshTokenDBO, cancellationToken);
      await _dbContext.SaveChangesAsync(cancellationToken);

      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Secure = true, // only send on HTTPS
        SameSite = SameSiteMode.Strict,
        Expires = refreshTokenDBO.ExpiresAt,
      };

      _httpContextAccessor.HttpContext?.Response.Cookies.Append(CookieConstants.RefreshTokenCookieName, refreshToken, cookieOptions);
    }

    private string GenerateSecureSevenDigitCodeToVerifyEmail()
    {
      byte[] randomNumber = new byte[4];

      RandomNumberGenerator.Fill(randomNumber);

      int code = BitConverter.ToInt32(randomNumber, 0) % 10000000;
      if (code < 1000000)
      {
        code += 1000000;
      }
      return code.ToString("D7");
    }
  }
}
