using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMax.Domain.Entities
{
    public class Episode
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime? ReleaseDate { get; set; }
        public int SeasonId { get; set; }
        public Season? Season { get; set; }
    }
}
