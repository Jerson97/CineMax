using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CineMax.Domain.Entities;

namespace Cinemax.Application.DTOs
{
    public class ReviewDto
    {
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
