using FluentValidation;
using ReactifyBlog.Business.Constants;
using ReactifyBlog.Business.Constants.ErrorConstants.Exceptions;
using ReactifyBlog.Business.DTOs.Auth;

namespace ReactifyBlog.Business.Validators.Auth;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
  public ConfirmEmailRequestValidator()
  {
    RuleFor(request => request.Email)
      .NotEmpty()
      .WithErrorCode(AuthServiceErrorConstants.ConfirmEmailRequiredEmailErrorCode)
      .WithMessage(AuthServiceErrorConstants.ConfirmEmailRequiredEmailErrorMessage);

    RuleFor(request => request.Email)
      .EmailAddress()
      .WithErrorCode(AuthServiceErrorConstants.ConfirmEmailFormatErrorCode)
      .WithMessage(AuthServiceErrorConstants.ConfirmEmailFormatErrorMessage);

    RuleFor(request => request.ConfirmationCode)
      .Length(NumericConstants.Six)
      .WithErrorCode(AuthServiceErrorConstants.ConfirmEmailConfirmationCodeLengthErrorCode)
      .WithMessage(AuthServiceErrorConstants.ConfirmEmailConfirmationCodeLengthErrorMessage);
  }
}