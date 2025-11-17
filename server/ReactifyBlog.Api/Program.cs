using ReactifyBlog.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDatabase(builder.Configuration)
    .AddApplicationCookie()
    .AddSwagger()
    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.UseSwaggerDocumentation();
app.UseCustomExceptionHandling()
    .UseHttpsRedirection()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllers();

app.Run();
