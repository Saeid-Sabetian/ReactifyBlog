using ReactifyBlog.Business.DTOs.Auth;
using FluentValidation;

namespace ReactifyBlog.Business.Validators.Auth
{
	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(x => x.Email)
					.NotEmpty().WithMessage("Email is required.")
					.EmailAddress().WithMessage("Invalid email format.").WithState(_ => 403);

			RuleFor(x => x.Password)
					.NotEmpty().WithMessage("Password is required.")
					.MinimumLength(6).WithMessage("Password must be at least 6 characters long.").WithState(_ => 403);
		}
	}
}