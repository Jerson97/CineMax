using System;
using System.Collections.Generic;
using System.Linq;
using Cinemax.Application.Interfaces;
using Cinemax.Persistence.Repositories;
using CineMax.Domain;
using Microsoft.AspNetCore.Identity;
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

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<CineMaxDbContext>());
            services.AddScoped<IMovieRepository, MovieRepository>();

            return services;
        }
    }
}
