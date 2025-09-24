using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinemax.Application.Common
{
    public class DataCollection<T>
    {
        public bool HasItems => Items.Any();

        public int Total { get; set; }
        public int Page { get; set; }
        public int Pages { get; set; }
        public List<T> Items { get; set; }
    }
}
