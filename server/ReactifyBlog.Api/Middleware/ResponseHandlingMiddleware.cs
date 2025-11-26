using System.Text.Json;
using FluentValidation;
using ReactifyBlog.Business.DTOs;
using ReactifyBlog.Business.Exceptions;

namespace ReactifyBlog.Api.Middleware
{
	public class ResponseHandlingMiddleware
	{
		private readonly RequestDelegate _next;

		public ResponseHandlingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			var originalBodyStream = httpContext.Response.Body;
			var responseBody = new MemoryStream();
			httpContext.Response.Body = responseBody;

			try
			{
				await _next(httpContext);

				httpContext.Response.Body = originalBodyStream;

				if (httpContext.Response.StatusCode >= 200 && httpContext.Response.StatusCode <= 300)
				{
					responseBody.Seek(0, SeekOrigin.Begin);
					var responseText = await new StreamReader(responseBody).ReadToEndAsync();

					object? data = null;
					if (!string.IsNullOrWhiteSpace(responseText))
					{
						try
						{
							data = JsonSerializer.Deserialize<dynamic>(responseText,
									new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
						}
						catch (JsonException)
						{
							data = responseText;
						}
					}

					var appResponse = new AppResponse<object> { Data = data };

					httpContext.Response.ContentType = "application/json";
					await httpContext.Response.WriteAsync(
							JsonSerializer.Serialize(appResponse,
									new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
				}
			}
			catch (ReactifyBlogException ex)
			{
				httpContext.Response.Body = originalBodyStream;
				httpContext.Response.ContentType = "application/json";
				httpContext.Response.StatusCode = ex.StatusCode;

				var errorResponse = new AppResponse<object>
				{
					Error = new ErrorInfo
					{
						Code = ex.ErrorCode,
						Message = ex.Message,
						Service = ex.ServiceName
					}
				};

				await httpContext.Response.WriteAsync(
						JsonSerializer.Serialize(errorResponse,
								new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
			}
			catch (ValidationException ex)
			{
				httpContext.Response.Body = originalBodyStream;

				var statusCode = ex.Errors
						.Select(e => e.CustomState)
						.OfType<int>()
						.FirstOrDefault();

				httpContext.Response.StatusCode = statusCode == 0 ? 400 : statusCode;

				await httpContext.Response.WriteAsJsonAsync(new
				{
					errors = ex.Errors.Select(e => new
					{
						e.PropertyName,
						e.ErrorMessage
					})
				});
			}
			catch (Exception)
			{
				httpContext.Response.Body = originalBodyStream;
				httpContext.Response.ContentType = "application/json";
				httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

				var errorResponse = new AppResponse<object>
				{
					Error = new ErrorInfo
					{
						Code = "UNEXPECTED_ERROR",
						Message = "An unexpected internal server error occurred.",
						Service = "Global"
					}
				};

				await httpContext.Response.WriteAsync(
						JsonSerializer.Serialize(errorResponse,
								new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
			}
			finally
			{
				responseBody.Dispose();
			}
		}
	}
}