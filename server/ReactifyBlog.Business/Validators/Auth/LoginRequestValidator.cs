using FluentValidation;
using ReactifyBlog.Business.Constants;
using ReactifyBlog.Business.Constants.ErrorConstants.Exceptions;
using ReactifyBlog.Business.DTOs.Auth;

namespace ReactifyBlog.Business.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
  public LoginRequestValidator()
  {
    RuleFor(request => request.Email)
        .NotEmpty()
        .WithErrorCode(AuthServiceErrorConstants.LoginRequiredEmailErrorCode)
        .WithMessage(AuthServiceErrorConstants.LoginRequiredEmailErrorMessage);

    RuleFor(request => request.Password)
        .NotEmpty()
        .WithErrorCode(AuthServiceErrorConstants.LoginRequiredPasswordErrorCode)
        .WithMessage(AuthServiceErrorConstants.LoginRequiredPasswordErrorMessage);

    RuleFor(request => request.Password)
      .MinimumLength(NumericConstants.Six)
      .WithErrorCode(AuthServiceErrorConstants.LoginInvalidPasswordLengthErrorCode)
      .WithMessage(AuthServiceErrorConstants.LoginInvalidPasswordLengthErrorMessage);
  }
}