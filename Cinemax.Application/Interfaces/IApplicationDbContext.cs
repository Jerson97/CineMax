using CineMax.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Movie> Movies { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Review> Reviews { get; set; }
        DbSet<Favorite> Favorites { get; set; }
        DbSet<Series> Series { get; set; }
        DbSet<MovieCategory> MovieCategories { get; set; }
        DbSet<SeriesCategory> SeriesCategories { get; set; }
        DbSet<Actor> Actors { get; set; }
        DbSet<Director> Directors { get; set; }
        DbSet<MovieActor> MovieActor { get; set; }
        DbSet<MovieDirector> MovieDirectors { get; set; }
        DbSet<SeriesActor> SeriesActor { get; set; }
        DbSet<SeriesDirector> SeriesDirectors { get; set; }
        DbSet<Season> Seasons { get; set; }
        DbSet<Episode> Episodes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
