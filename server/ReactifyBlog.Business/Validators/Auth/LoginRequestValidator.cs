using FluentValidation;
using ReactifyBlog.Business.DTOs.Auth;
using System.Net;

namespace ReactifyBlog.Business.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
  public LoginRequestValidator()
  {
    RuleFor(x => x.Email)
        .NotEmpty().WithMessage("Email is required.")
        .EmailAddress().WithMessage("Invalid email format.").WithState(_ => HttpStatusCode.BadRequest);

    RuleFor(x => x.Password)
        .NotEmpty().WithMessage("Password is required.")
        .MinimumLength(6).WithMessage("Password must be at least 6 characters long.").WithState(_ => HttpStatusCode.BadRequest);
  }
}