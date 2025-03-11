using VotingApp.API.Exceptions;
using UnauthorizedAccessException = VotingApp.API.Exceptions.UnauthorizedAccessException;

namespace VotingApp.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode;
            string errorMessage = ex.Message;

            switch (ex)
            {
                case NotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case ConflictException:
                    statusCode = StatusCodes.Status409Conflict;
                    break;
                case BadRequestException:
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case UnauthorizedAccessException:
                    statusCode = StatusCodes.Status401Unauthorized;
                    break;

                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    errorMessage = ex.Message;
                    break;
            }

            var response = new { error = true, code = statusCode, errorMessage };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
