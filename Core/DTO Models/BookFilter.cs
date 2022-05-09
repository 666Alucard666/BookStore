using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO_Models
{
    public class BookFilter
    {
        public string Author { get; set; }

        public string Genre { get; set; }

        public double StartPrice { get; set; }

        public double EndPrice { get; set; }
    }
}
