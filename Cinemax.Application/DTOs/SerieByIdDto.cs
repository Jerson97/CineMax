using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemax.Application.DTOs
{
    public class SerieByIdDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Season { get; set; }

        public List<CategoryDto>? CategoryList { get; set; }
        public List<ActorDto>? ActorList { get; set; }
        public List<DirectorDto>? DirectorList { get; set; }
        public List<SeasonDto>? SeasonList { get; set; }
    }
}
