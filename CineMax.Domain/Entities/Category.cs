namespace CineMax.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<MovieCategory>? MovieCategories { get; set; }
        public ICollection<SeriesCategory>? SeriesCategories { get; set; }
    }
}
