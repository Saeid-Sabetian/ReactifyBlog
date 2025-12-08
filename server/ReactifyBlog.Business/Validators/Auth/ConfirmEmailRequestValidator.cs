using FluentValidation;
using ReactifyBlog.Business.DTOs.Auth;
using System.Net;

namespace ReactifyBlog.Business.Validators.Auth;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
  public ConfirmEmailRequestValidator()
  {
    RuleFor(request => request.Email).NotEmpty().WithMessage("Email is required.")
      .EmailAddress().WithMessage("Invalid email format.").WithState(_ => HttpStatusCode.BadRequest);

    RuleFor(request => request.ConfirmationCode)
      .Length(6)
      .WithMessage("The confirmation code must be a 6-digit number.")
      .WithState(_ => HttpStatusCode.BadRequest);
  }
}