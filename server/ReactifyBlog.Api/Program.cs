using ReactifyBlog.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
		.AddDatabase(builder.Configuration)
		.AddApplicationCookie()
		.AddSwagger()
		.AddAutoMapper()
		.AddApplicationServices()
		.AddFluentValidation()
		.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerDocumentation();
}

app.UseStaticFiles()
	 .UseResponseExceptionHandling()
	 .UseHttpsRedirection()
	 .UseRouting()
	 .UseAuthentication()
	 .UseAuthorization();

app.MapControllers();

app.Run();
