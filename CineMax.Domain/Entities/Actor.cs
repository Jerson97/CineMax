namespace CineMax.Domain.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<MovieActor> Movies { get; set; } = new List<MovieActor>();
        public ICollection<SeriesActor> Series { get; set; } = new List<SeriesActor>();
    }
}
