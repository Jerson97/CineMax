namespace CineMax.Domain.Entities
{
    public class SeriesDirector
    {
        public int SeriesId { get; set; }
        public Series? Series { get; set; }
        public int DirectorId { get; set; }
        public Director? Director { get; set; }
    }
}
