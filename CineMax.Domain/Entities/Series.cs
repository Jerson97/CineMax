namespace CineMax.Domain.Entities
{
    public class Series
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<SeriesCategory> SeriesCategories { get; set; } = new List<SeriesCategory>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<SeriesActor> SeriesActor { get; set; } = new List<SeriesActor>();
        public ICollection<SeriesDirector> SeriesDirectors { get; set; } = new List<SeriesDirector>();
        public ICollection<Season> Seasons { get; set; } = new List<Season>();
    }
}
