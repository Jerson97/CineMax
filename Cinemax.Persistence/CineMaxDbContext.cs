using Cinemax.Application.Interfaces;
using CineMax.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cinemax.Persistence
{
    public class CineMaxDbContext: IdentityDbContext<User, IdentityRole<int>, int>, IApplicationDbContext
    {
        public CineMaxDbContext (DbContextOptions<CineMaxDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MovieCategory>().HasKey(ci => new { ci.MovieId, ci.CategoryId });
            builder.Entity<SeriesCategory>().HasKey(ci => new { ci.SeriesId, ci.CategoryId });
            builder.Entity<MovieDirector>().HasKey(ci => new { ci.MovieId, ci.DirectorId });
            builder.Entity<SeriesDirector>().HasKey(ci => new { ci.SeriesId, ci.DirectorId });
            builder.Entity<MovieActor>().HasKey(ci => new { ci.MovieId, ci.ActorId });
            builder.Entity<SeriesActor>().HasKey(ci => new { ci.SeriesId, ci.ActorId });
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Favorite> Favorites { get; set; } = null!;
        public DbSet<Series> Series { get; set; } = null!;
        public DbSet<Director> Directors { get; set; } = null!;
        public DbSet<Actor> Actors { get; set; } = null!;
        public DbSet<MovieCategory> MovieCategories { get; set; } = null!;
        public DbSet<SeriesCategory> SeriesCategories { get; set; } = null!;
        public DbSet<MovieDirector> MovieDirectors { get; set; } = null!;
        public DbSet<SeriesDirector> SeriesDirectors { get; set; } = null!;
        public DbSet<SeriesActor> SeriesActor { get; set; } = null!;
        public DbSet<MovieActor> MovieActor { get; set; }
        public DbSet<Season> Seasons { get; set; } = null!;
        public DbSet<Episode> Episodes { get; set; } = null!;
    }
}
