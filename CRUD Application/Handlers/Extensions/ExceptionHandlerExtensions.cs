using CRUD_Application.Handlers.Middleware;

namespace CRUD_Application.Handlers.Extensions
{
    public static class ExceptionHandlerExtensions
    {
        public static IApplicationBuilder
            UseGlobalExceptionHandler(
                this IApplicationBuilder app)
        {
            return app.UseMiddleware<
                GlobalExceptionHandler>();
        }
    }
}
