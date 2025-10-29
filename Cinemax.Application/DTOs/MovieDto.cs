﻿namespace Cinemax.Application.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string? ImageUrl { get; set; }
    }
}
