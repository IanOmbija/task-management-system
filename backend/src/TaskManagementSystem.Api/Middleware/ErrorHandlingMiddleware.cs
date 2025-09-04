using System.Net;
using System.Text.Json;


namespace TaskManagamentSystem.Api.Middleware;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ErrorHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext contxt)
    {
        try
        {
            await _next(contxt);
        }
        catch (UnauthorizedAccessException ex)
        {
            await Write(contxt, HttpStatusCode.Unauthorized, ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await Write(contxt, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            await Write(contxt, HttpStatusCode.NotFound, ex.Message);
        }
        catch (Exception)
        {
            await Write(contxt, HttpStatusCode.InternalServerError, "An unexpcted error occurred.");
        }
    }

    private static Task Write(HttpContext contxt, HttpStatusCode code, string message)
    {
        contxt.Response.ContentType = "application/json";
        contxt.Response.StatusCode = (int)code;
        var payload = JsonSerializer.Serialize(new { error = message });
        return contxt.Response.WriteAsync(payload);

     }
}