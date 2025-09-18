using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMax.Domain.Entities
{
    public class SeriesCategory
    {
        public int SeriesId { get; set; }
        public Series? Series { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
