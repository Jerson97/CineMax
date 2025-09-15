using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineMax.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        public ICollection<MovieCategory> MovieCategories { get; set; }
        public ICollection<SeriesCategory> SeriesCategories { get; set; }
    }
}
