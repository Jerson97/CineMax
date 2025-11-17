namespace CineMax.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public bool IsActive { get; set; } = true;
        public string? ImageUrl { get; set; }
        public string? TrailerUrl { get; set; }

        public ICollection<MovieCategory> MovieCategories { get; set; } = new List<MovieCategory>();
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<MovieActor> MovieActor { get; set; } = new List<MovieActor>();
        public ICollection<MovieDirector> MovieDirectors { get; set; } = new List<MovieDirector>();
    }
}
