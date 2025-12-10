using FluentValidation;
using ReactifyBlog.Business.Constants;
using ReactifyBlog.Business.Constants.ErrorConstants.Exceptions;
using ReactifyBlog.Business.DTOs.Auth;

namespace ReactifyBlog.Business.Validators.Auth;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
  public ChangePasswordRequestValidator()
  {
    RuleFor(request => request.OldPassword)
        .NotEmpty()
        .WithErrorCode(AuthServiceErrorConstants.ChangePasswordRequiredOldPasswordErrorCode)
        .WithMessage(AuthServiceErrorConstants.ChangePasswordRequiredOldPasswordErrorMessage);

    RuleFor(request => request.NewPassword)
        .NotEmpty()
        .WithErrorCode(AuthServiceErrorConstants.ChangePasswordRequiredNewPasswordErrorCode)
        .WithMessage(AuthServiceErrorConstants.ChangePasswordRequiredNewPasswordErrorMessage);

    RuleFor(request => request.NewPassword)
        .MinimumLength(NumericConstants.Six)
        .WithErrorCode(AuthServiceErrorConstants.ChangePasswordNewPasswordLengthErrorCode)
        .WithMessage(AuthServiceErrorConstants.ChangePasswordNewPasswordLengthErrorMessage);

    RuleFor(request => request.NewPassword)
        .Matches(RegexConstants.PasswordLowercase)
        .WithErrorCode(AuthServiceErrorConstants.ChangePasswordNewPasswordRequiresLowerErrorCode)
        .WithMessage(AuthServiceErrorConstants.ChangePasswordNewPasswordRequiresLowerErrorMessage);

    RuleFor(request => request.NewPassword)
        .Matches(RegexConstants.PasswordUppercase)
        .WithErrorCode(AuthServiceErrorConstants.ChangePasswordNewPasswordRequiresUpperErrorCode)
        .WithMessage(AuthServiceErrorConstants.ChangePasswordNewPasswordRequiresUpperErrorMessage);

    RuleFor(request => request.NewPassword)
        .Matches(RegexConstants.PasswordDigit)
        .WithErrorCode(AuthServiceErrorConstants.ChangePasswordNewPasswordRequiresDigitErrorCode)
        .WithMessage(AuthServiceErrorConstants.ChangePasswordNewPasswordRequiresDigitErrorMessage);

    RuleFor(request => request.NewPassword)
        .Matches(RegexConstants.PasswordSpecialCharacter)
        .WithErrorCode(AuthServiceErrorConstants.ChangePasswordNewPasswordRequiresSpecialCharErrorCode)
        .WithMessage(AuthServiceErrorConstants.ChangePasswordNewPasswordRequiresSpecialCharErrorMessage);

    RuleFor(request => request.ConfirmNewPassword)
        .NotEmpty()
        .WithErrorCode(AuthServiceErrorConstants.ChangePasswordConfirmNewPasswordIsRequiredErrorCode)
        .WithMessage(AuthServiceErrorConstants.ChangePasswordConfirmNewPasswordIsRequiredErrorMessage);

    RuleFor(request => request.ConfirmNewPassword)
        .Equal(request => request.NewPassword)
        .WithErrorCode(AuthServiceErrorConstants.ChangePasswordConfirmNewPasswordMismatchErrorCode)
        .WithMessage(AuthServiceErrorConstants.ChangePasswordConfirmNewPasswordMismatchErrorMessage);
  }
}