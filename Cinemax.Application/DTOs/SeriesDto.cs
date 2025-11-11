namespace Cinemax.Application.DTOs
{
    public class SeriesDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Type { get; set; } = "series";
        public string? ImageUrl { get; set; }
    }
}
