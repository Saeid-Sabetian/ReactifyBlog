using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ReactifyBlog.Business.Common;
using ReactifyBlog.Business.Constants;
using ReactifyBlog.Business.Constants.ErrorConstants;
using ReactifyBlog.Business.Constants.ErrorConstants.Exceptions;
using ReactifyBlog.Business.Contracts.Services;
using ReactifyBlog.Business.DTOs.Auth;
using ReactifyBlog.Business.Exceptions;
using ReactifyBlog.Data.Data;
using ReactifyBlog.Data.Models;
using System.Security.Cryptography;

namespace ReactifyBlog.Business.Services;

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

  public async Task RegisterUserAsync(RegisterRequest request)
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

      return;
    }

    var identityErrorCode = result.Errors.Select(err => err.Code).First();

    switch (identityErrorCode)
    {
      case IdentityErrorCodeConstants.DuplicateEmail:
      case IdentityErrorCodeConstants.DuplicateUserName:
        throw new ReactifyBlogException(
            AuthServiceErrorConstants.RegisterDuplicateEmailErrorCode,
            (int)System.Net.HttpStatusCode.BadRequest,
            AuthServiceErrorConstants.RegisterDuplicateEmailErrorMessage
        );

      default:
        throw new ReactifyBlogException(
            AuthServiceErrorConstants.RegisterFailedErrorCode,
            (int)System.Net.HttpStatusCode.BadRequest,
            AuthServiceErrorConstants.RegisterFailedErrorMessage
        );
    }
  }

  public async Task LoginUserAsync(LoginRequest request, CancellationToken cancellationToken)
  {
    var isLoginSuccessful = false;

    var user = await _userManager.FindByEmailAsync(request.Email);

    if (user is null)
    {
      throw new ReactifyBlogException(
        AuthServiceErrorConstants.LoginInvalidCredentialsErrorCode,
        (int)System.Net.HttpStatusCode.BadRequest,
        AuthServiceErrorConstants.LoginInvalidCredentialsErrorMessage);
    }

    var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);

    if (result.IsLockedOut)
    {
      var utcNow = DateTime.UtcNow;

      var remaining = user.LockoutEnd.Value - utcNow;

      var minutesLeftToUnlock = (int)remaining.TotalMinutes;

      minutesLeftToUnlock = minutesLeftToUnlock == NumericConstants.Zero ? NumericConstants.One : minutesLeftToUnlock;

      throw new ReactifyBlogException(
              AuthServiceErrorConstants.LockedOutErrorCode,
              (int)System.Net.HttpStatusCode.BadRequest,
              string.Format(AuthServiceErrorConstants.LockedOutErrorMessage, minutesLeftToUnlock));
    }

    if (result.IsNotAllowed)
    {
      throw new ReactifyBlogException(
                AuthServiceErrorConstants.LoginUserNotConfirmedErrorCode,
                (int)System.Net.HttpStatusCode.BadRequest,
                AuthServiceErrorConstants.LoginUserNotConfirmedErrorMessage);
    }

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
                (int)System.Net.HttpStatusCode.BadRequest,
                AuthServiceErrorConstants.LoginFailedErrorMessage);
    }
  }

  public async Task<bool> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken)
  {
    var user = await _userManager.FindByIdAsync(request.UserId.ToString());
    if (user == null)
    {
      throw new ReactifyBlogException(AuthServiceErrorConstants.ChangePasswordFailedErrorCode, (int)System.Net.HttpStatusCode.NotFound, AuthServiceErrorConstants.ChangePasswordFailedErrorMessage);
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

    string sixDigitCode = NumericHelper.GenerateRandomInt(NumericConstants.OneHundredThousand, NumericConstants.OneMillion).ToString();
    user.ConfirmEmailCode = sixDigitCode;
    user.ConfirmEmailExpiration = DateTimeOffset.UtcNow.AddMinutes(15);
    await _userManager.UpdateAsync(user);

    if (user.Email != null)
    {
      await _emailService.SendEmailAsync(user.Email, EmailConstants.RecoverPasswordSubject, $"Your password recovery code is: {sixDigitCode}");
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
                (int)System.Net.HttpStatusCode.NotFound,
                AuthServiceErrorConstants.ConfirmEmailUserDoesNotExistErrorMessage);
    }

    if (user.EmailConfirmed)
    {
      throw new ReactifyBlogException(
            AuthServiceErrorConstants.ConfirmEmailAlreadyConfirmedErrorCode,
            (int)System.Net.HttpStatusCode.NotAcceptable,
            AuthServiceErrorConstants.ConfirmEmailAlreadyConfirmedErrorMessage);
    }

    var utcNow = DateTime.UtcNow;
    if (utcNow > user.ConfirmEmailExpiration)
    {
      throw new ReactifyBlogException(
            AuthServiceErrorConstants.ConfirmEmailExpiredErrorCode,
            (int)System.Net.HttpStatusCode.BadRequest,
            AuthServiceErrorConstants.ConfirmEmailExpiredErrorMessage);
    }

    if (!request.ConfirmationCode.Equals(user.ConfirmEmailCode))
    {
      throw new ReactifyBlogException(
                AuthServiceErrorConstants.ConfirmEmailInvalidCodeErrorCode,
                (int)System.Net.HttpStatusCode.BadRequest,
                AuthServiceErrorConstants.ConfirmEmailInvalidCodeErrorMessage);
    }

    user.EmailConfirmed = true;
    user.ConfirmEmailCode = null;
    user.ConfirmEmailExpiration = null;
    await _userManager.UpdateAsync(user);
  }

  public async Task LogoutUserAsync()
  {
    await _signInManager.SignOutAsync();
  }

  public async Task GenerateAndStoreRefreshTokenAsync(UserDBO user, CancellationToken cancellationToken)
  {
    var randomNumber = new byte[NumericConstants.SixtyFour];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomNumber);
    var refreshToken = Convert.ToBase64String(randomNumber);

    var hashedRefreshToken = _userManager.PasswordHasher.HashPassword(user, refreshToken);

    var utcNow = DateTime.UtcNow;
    var refreshTokenDBO = new RefreshTokenDBO
    {
      TokenHash = hashedRefreshToken,
      UserId = user.Id,
      ExpiresAt = utcNow.AddDays(NumericConstants.Seven),
      CreatedAt = utcNow
    };

    await _dbContext.RefreshTokens.AddAsync(refreshTokenDBO, cancellationToken);
    await _dbContext.SaveChangesAsync(cancellationToken);

    var cookieOptions = new CookieOptions
    {
      HttpOnly = true,
      Secure = true,
      SameSite = SameSiteMode.Strict,
      Expires = refreshTokenDBO.ExpiresAt,
    };

    _httpContextAccessor.HttpContext?.Response.Cookies.Append(CookieConstants.RefreshTokenCookieName, refreshToken, cookieOptions);
  }
}
