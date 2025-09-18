using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMax.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }
        public int SeriesId { get; set; }
        public Series? Series { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }

    }
}
