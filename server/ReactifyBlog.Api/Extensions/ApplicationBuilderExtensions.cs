using ReactifyBlog.Api.Middleware;

namespace ReactifyBlog.Api.Extensions
{
	public static class ApplicationBuilderExtensions
	{
		public static IApplicationBuilder UseCustomExceptionHandling(this IApplicationBuilder app)
		{
			return app.UseMiddleware<ExceptionHandlingMiddleware>();
		}

		public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(options =>
			{
				options.SwaggerEndpoint("/swagger/v1/swagger.json", "ReactifyBlog API V1");
				options.InjectStylesheet("/swagger-ui/dark.css");
			});
			return app;
		}
	}
}
