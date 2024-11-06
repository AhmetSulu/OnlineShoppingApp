using OnlineShoppingApp.WebApi.Middlewares;

namespace OnlineShoppingApp.WebApi.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseMaintenanceMode(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MaintenanceMiddleware>();
        }

        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
