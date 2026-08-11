using CRUD_Application.Handlers.Exceptions;
using CRUD_Application.Handlers.Responses;
using System.Net;
using System.Text.Json;

namespace CRUD_Application.Handlers.Middleware
{

    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(
            RequestDelegate next,
            ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                if (!context.Response.HasStarted)
                {
                    await HandleExceptionAsync(context, ex);
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                BadRequestException => HttpStatusCode.BadRequest,
                ConflictException => HttpStatusCode.Conflict,
                _ => HttpStatusCode.InternalServerError
            };

            context.Response.Clear();
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>
            {
                Success = false,
                Message = exception switch
                {
                    NotFoundException => exception.Message,
                    BadRequestException => exception.Message,
                    ConflictException => exception.Message,
                    _ => "An unexpected error occurred."
                },
                Errors = exception switch
                {
                    NotFoundException => new[]
                    {
                        new ApiError
                        {
                            Code = "NOT_FOUND",
                            Message = exception.Message
                        }
                    },

                    BadRequestException => new[]
                    {
                        new ApiError
                        {
                            Code = "BAD_REQUEST",
                            Message = exception.Message
                        }
                    },

                    ConflictException => new[]
                    {
                        new ApiError
                        {
                            Code = "CONFLICT",
                            Message = exception.Message
                        }
                    },

                    _ => null
                },
                TraceId = context.TraceIdentifier
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
