using CineMax.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Cinemax.Persistence
{
    public class SeedData
    {
        public static async Task InsertData(CineMaxDbContext context, UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole<int>("ADMIN"));
                await roleManager.CreateAsync(new IdentityRole<int>("USER"));
            }

            if (!userManager.Users.Any())
            {
                var user = new User
                {
                    Name = "Jerson",
                    LastName = "Ramirez",
                    Email = "jersonramsoto@gmail.com",
                    UserName = "jerson.ramirez"
                };

                await userManager.CreateAsync(user, "PasswordJersonRamirez123$");
                await userManager.AddToRoleAsync(user, "ADMIN");
            }

            if (!context.Categories.Any())
            {
                await context.Categories!.AddRangeAsync(
                        new Category { Name = "Terror" },
                        new Category { Name = "Drama" },
                        new Category { Name = "Comedia" },
                        new Category { Name = "Accion" }
                    );
                await context.SaveChangesAsync();
            }

            if (!context.Movies.Any())
            {
                await context.Movies!.AddRangeAsync(
                    new Movie
                    {
                        Title = "El Aro",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        ReleaseDate = new DateTime(2001, 10, 15),
                        Duration = 2
                    },

                    new Movie
                    {
                        Title = "Los Indestructibles",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        ReleaseDate = new DateTime(2006, 11, 15),
                        Duration = 3
                    });

                await context.SaveChangesAsync();

            }

            if (!context.Series.Any())
            {
                await context.Series!.AddRangeAsync(
                    new Series
                    {
                        Title = "Los SimpSon",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        ReleaseDate = new DateTime(2002, 04, 01),
                        Season = 2
                    },

                    new Series
                    {
                        Title = "Mil oficios",
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                        ReleaseDate = new DateTime(1998, 10, 15),
                        Season = 5
                    });

                await context.SaveChangesAsync();

            }

            if (!context.MovieCategories!.Any())
            {
                await context.MovieCategories!.AddRangeAsync(
                    new MovieCategory { CategoryId = 1, MovieId = 1 },
                    new MovieCategory { CategoryId = 4, MovieId = 2 });

                await context.SaveChangesAsync();
            }

            if (!context.SeriesCategories!.Any())
            {
                await context.SeriesCategories!.AddRangeAsync(
                    new SeriesCategory { CategoryId = 3, SeriesId = 1 },
                    new SeriesCategory { CategoryId = 3, SeriesId = 2 });

                await context.SaveChangesAsync();
            }

        }
    }
}
