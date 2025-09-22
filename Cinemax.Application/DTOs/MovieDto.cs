using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineMax.Domain.Entities;

namespace Cinemax.Application.DTOs
{
    public class MovieDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }

        public List<CategoryDto> CategoryList { get; set; } 
    }
}
