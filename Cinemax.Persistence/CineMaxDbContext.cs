using System;
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
        }

        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Favorite> Favorites { get; set; } = null!;
        public DbSet<Series> Series { get; set; } = null!;  
        public DbSet<MovieCategory> MovieCategories { get; set; } = null!;
        public DbSet<SeriesCategory> SeriesCategories { get; set; } = null!;
    }
}
