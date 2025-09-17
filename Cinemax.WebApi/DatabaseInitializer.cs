using CineMax.Domain;
using Cinemax.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.WebApi
{
    public static class DatabaseInitializer
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<CineMaxDbContext>();
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();


            // Ejecutar migraciones
            await context.Database.MigrateAsync();

            // Insertar datos iniciales
            await SeedData.InsertData(context, userManager, roleManager);
        }
    }
}
