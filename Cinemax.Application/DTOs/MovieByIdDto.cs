namespace Cinemax.Application.DTOs
{
    public class MovieByIdDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string? ImageUrl { get; set; }

        public List<CategoryDto>? CategoryList { get; set; }
        public List<ActorDto>? ActorList { get; set; }
        public List<DirectorDto>? DirectorList { get; set; }
    }
}
