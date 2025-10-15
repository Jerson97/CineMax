namespace Cinemax.Application.DTOs
{
    public class SeriesDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Season { get; set; }

        public List<CategoryDto>? CategoryList { get; set; }
        public List<ActorDto>? ActorList { get; set; }
        public List<DirectorDto>? DirectorList { get; set; }
    }
}
