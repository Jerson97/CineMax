namespace CineMax.Domain.Entities
{
    public class SeriesActor
    {
        public int SeriesId { get; set; }
        public Series? Series { get; set; }
        public int ActorId { get; set; }
        public Actor? Actor { get; set; }
    }
}
