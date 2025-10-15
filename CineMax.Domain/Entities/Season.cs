using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMax.Domain.Entities
{
    public class Season
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int SeriesId { get; set; }
        public Series? Series { get; set; }
        public ICollection<Episode> Episodes { get; set; } = new List<Episode>();
    }
}
