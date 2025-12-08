using FluentValidation;
using ReactifyBlog.Business.Constants.ErrorConstants.Exceptions;
using ReactifyBlog.Business.DTOs.Auth;
using System.Net;

namespace ReactifyBlog.Business.Validators.Auth
{
	public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
	{
		public RegisterRequestValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
				.WithErrorCode(AuthServiceErrorConstants.RegRequiredEmailErrorCode)
				.WithMessage(AuthServiceErrorConstants.RegRequiredEmailErrorMessage)
				.WithState(_ => HttpStatusCode.BadRequest);

      RuleFor(request => request.Email)
				.EmailAddress()
				.WithErrorCode(AuthServiceErrorConstants.RegInvalidEmailFormatErrorCode)
				.WithMessage(AuthServiceErrorConstants.RegInvalidEmailFormatErrorMessage)
				.WithState(_ => HttpStatusCode.BadRequest);

      RuleFor(x => x.Password)
				 .NotEmpty()
				 .WithErrorCode(AuthServiceErrorConstants.RegRequiredPasswordErrorCode)
				 .WithMessage(AuthServiceErrorConstants.RegRequiredPasswordErrorMessage)					
				 .WithState(_ => HttpStatusCode.BadRequest);

			RuleFor(request => request.Password)
				.MinimumLength(6)
				.WithErrorCode(AuthServiceErrorConstants.RegInvalidPasswordLengthErrorCode)
				.WithMessage(AuthServiceErrorConstants.RegInvalidPasswordLengthErrorMessage)
        .WithState(_ => HttpStatusCode.BadRequest);


      RuleFor(x => x.ConfirmPassword)
				.Equal(x => x.Password)
				.WithErrorCode(AuthServiceErrorConstants.RegPasswordMismatchErrorCode)
				.WithMessage(AuthServiceErrorConstants.RegPasswordMismatchErrorMessage)
				.WithState(_ => HttpStatusCode.BadRequest);
		}
	}
}