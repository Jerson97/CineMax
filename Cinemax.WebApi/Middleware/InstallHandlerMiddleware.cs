using Cinemax.WebApi.Middleware.ErrorMiddlewares;

namespace Cinemax.WebApi.Middleware
{
    public static class InstallHandlerMiddleware
    {
        public static IApplicationBuilder UseHandlerUsers(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlerMiddleware>();
        }
    }
}
