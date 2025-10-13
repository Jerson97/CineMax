using System.Reflection;
using Cinemax.Application.Behaviors;
using Cinemax.Application.Features.Movies.Commands.Create;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cinemax.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddValidatorsFromAssemblies(new[] { typeof(MovieCreateValidation).Assembly });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviors<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }
    }
}
