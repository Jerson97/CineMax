using Cinemax.Application.Interfaces;
using Cinemax.Application.Interfaces.Token;
using Cinemax.Persistence.Repositories;
using Cinemax.Persistence.Repositories.Token;
using Cinemax.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cinemax.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CineMaxDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<CineMaxDbContext>());
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IDirectoryRepository, DirectorRepository>();
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<ISeriesRepository, SeriesRepository>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<IEpisodeRepository, EpisodeRepository>();
            services.AddScoped<ISearchRepository, SearchRepository>();
            services.AddScoped<IFavoriteRepository, FavoriteRepository>();
            services.AddScoped<IUserRepository,  UserRepository>();


            services.AddScoped<IBlobStorageService, BlobStorageService>();

            return services;
        }
    }
}
