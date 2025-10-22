namespace Cinemax.Application.DTOs
{
    public class EpisodeDto
    {
        public int Number { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }
    }
}
