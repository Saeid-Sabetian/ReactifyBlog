using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ReactifyBlog.Business.Filters
{
	public class ValidationFilter : IAsyncActionFilter
	{
		private readonly IServiceProvider _serviceProvider;

		public ValidationFilter(IServiceProvider serviceProvider)
		{
			_serviceProvider = serviceProvider;
		}

		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			foreach (var arg in context.ActionArguments.Values)
			{
				if (arg == null) continue;

				var type = arg.GetType();

				var validatorType = typeof(IValidator<>).MakeGenericType(type);
				var validator = _serviceProvider.GetService(validatorType) as IValidator;

				if (validator != null)
				{
					var validationContext = new ValidationContext<object>(arg);
					var result = await validator.ValidateAsync(validationContext);

					if (!result.IsValid)
					{
						throw new ValidationException(result.Errors);
					}
				}
			}

			await next();
		}
	}
}
