using FluentValidation;
using ReactifyBlog.Business.Constants;
using ReactifyBlog.Business.Constants.ErrorConstants.Exceptions;
using ReactifyBlog.Business.DTOs.Auth;

namespace ReactifyBlog.Business.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
  public RegisterRequestValidator()
  {
    RuleFor(x => x.Email)
         .NotEmpty()
         .WithErrorCode(AuthServiceErrorConstants.RegRequiredEmailErrorCode)
         .WithMessage(AuthServiceErrorConstants.RegRequiredEmailErrorMessage);

    RuleFor(request => request.Email)
         .EmailAddress()
         .WithErrorCode(AuthServiceErrorConstants.RegInvalidEmailFormatErrorCode)
         .WithMessage(AuthServiceErrorConstants.RegInvalidEmailFormatErrorMessage);

    RuleFor(request => request.Password)
         .NotEmpty()
         .WithErrorCode(AuthServiceErrorConstants.RegRequiredPasswordErrorCode)
         .WithMessage(AuthServiceErrorConstants.RegRequiredPasswordErrorMessage);

    RuleFor(request => request.Password)
         .MinimumLength(NumericConstants.Six)
         .WithErrorCode(AuthServiceErrorConstants.RegInvalidPasswordLengthErrorCode)
         .WithMessage(AuthServiceErrorConstants.RegInvalidPasswordLengthErrorMessage);

    RuleFor(request => request.ConfirmPassword)
         .Equal(request => request.Password)
         .WithErrorCode(AuthServiceErrorConstants.RegPasswordMismatchErrorCode)
         .WithMessage(AuthServiceErrorConstants.RegPasswordMismatchErrorMessage);

    RuleFor(request => request.Password)
        .Matches(RegexConstants.PasswordLowercase)
        .WithErrorCode(AuthServiceErrorConstants.RegPasswordRequiresLowerErrorCode)
        .WithMessage(AuthServiceErrorConstants.RegPasswordRequiresLowerErrorMessage);

    RuleFor(request => request.Password)
        .Matches(RegexConstants.PasswordUppercase)
        .WithErrorCode(AuthServiceErrorConstants.RegPasswordRequiresUpperErrorCode)
        .WithMessage(AuthServiceErrorConstants.RegPasswordRequiresUpperErrorMessage);

    RuleFor(request => request.Password)
        .Matches(RegexConstants.PasswordDigit)
        .WithErrorCode(AuthServiceErrorConstants.RegPasswordRequiresDigitErrorCode)
        .WithMessage(AuthServiceErrorConstants.RegPasswordRequiresDigitErrorMessage);

    RuleFor(request => request.Password)
        .Matches(RegexConstants.PasswordSpecialCharacter)
        .WithErrorCode(AuthServiceErrorConstants.RegPasswordRequiresSpecialCharErrorCode)
        .WithMessage(AuthServiceErrorConstants.RegPasswordRequiresSpecialCharErrorMessage);
  }
}