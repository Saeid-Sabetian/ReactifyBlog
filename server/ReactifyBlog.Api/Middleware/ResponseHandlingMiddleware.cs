using FluentValidation;
using ReactifyBlog.Business.DTOs;
using ReactifyBlog.Business.Exceptions;
using System.Net;
using System.Text.Json;

namespace ReactifyBlog.Api.Middleware;

public class ResponseHandlingMiddleware
{
  private readonly RequestDelegate _next;

  private JsonSerializerOptions options = new JsonSerializerOptions
  {
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
  };

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
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(appResponse, options));
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
        }
      };

      await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
    }
    catch (ValidationException ex)
    {
      httpContext.Response.Body = originalBodyStream;

      httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

      await httpContext.Response.WriteAsJsonAsync(new
      {
        errors = ex.Errors.Select(e => new
        {
          e.PropertyName,
          e.ErrorCode,
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

      await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, options));
    }
    finally
    {
      responseBody.Dispose();
    }
  }
}