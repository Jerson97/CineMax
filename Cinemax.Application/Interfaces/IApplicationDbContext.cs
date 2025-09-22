using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
