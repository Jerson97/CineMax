namespace CineMax.Domain.Entities
{
    public class Director
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<MovieDirector> Movies { get; set; } = new List<MovieDirector>();
        public ICollection<SeriesDirector> Series { get; set; } = new List<SeriesDirector>();
    }
}
