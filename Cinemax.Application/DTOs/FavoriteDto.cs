namespace Cinemax.Application.DTOs
{
    public class FavoriteDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; 
        public string? ImageUrl { get; set; }
    }
}
